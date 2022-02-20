using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Daniel.Utils;
using System.Reflection;
using System.Linq;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Daniel.Blazor.Services
{
   public abstract class DataService
   {
      public static JsonSerializerOptions JsonOptions { get; set; }
      public HttpClient Http { get; set; }

      protected AppManager AppManager { get; set; }
      protected NavigationManager NavigationManager { get; set; }

      protected AuthenticationService AuthService { get; set; }

      public abstract string RenewPath { get; }

      public DataService(HttpClient http, AppManager appManager, NavigationManager navigationManager, AuthenticationService authService)
      {
         AppManager = appManager;
         Http = http;
         NavigationManager = navigationManager;
         AuthService = authService;

         //Adicionar o token de autorização caso ele exista
         // var result = AppManager.GetAccessToken();
         // result.Wait();
         // string token = result.Result;
         // if (!string.IsNullOrEmpty(token))
         // {
         //     Console.WriteLine("Token encontrado");
         //     Http.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
         // }
         
            authService.GetUserLanguage().ContinueWith((task) =>
            {               
               Http.DefaultRequestHeaders.AcceptLanguage.Clear();
               Http.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(task.Result));
               Http.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
               //Http.DefaultRequestHeaders.Add("no-cache","false");
               //Http.DefaultRequestHeaders.Add("cache-control","public, max-age=300000");
               //Http.DefaultRequestHeaders.Add("pragma","cache");
               
               Console.WriteLine("AcceptLanguage="+Http.DefaultRequestHeaders.AcceptLanguage.First().Value.ToString());
            });
         

         JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
         JsonOptions.ReferenceHandler = ReferenceHandler.Preserve;
         JsonOptions.WriteIndented = true;
         JsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
         JsonOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
      }

      protected void NavigateUnauthorized()
      {
         NavigationManager.NavigateTo($"Unauthorized", true);
      }

      /// <summary>
      /// Obtém uma informação no servidor, transforma ela no objeto tipado
      /// </summary>
      /// <param name="path">Caminho da chamada da API</param>
      /// <param name="queryString">query string caso exista</param>
      /// <param name="NavigateNotFound">Navega para página de não encontrado caso o recurso não seja encontrado</param>
      /// <typeparam name="TObject"></typeparam>
      /// <returns></returns>
      public async Task<TObject> Get<TObject>(string path, string queryString = "")
      {
         var response = await Http.GetAsync(path + (string.IsNullOrEmpty(queryString) ? "" : ("?" + queryString)));

         await ProcessResponse(response);
         var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
         //TODO checar se a resposta foi ok, caso aconteça um erro reportar o erro

         var result = JsonSerializer.Deserialize<TObject>(apiResponse.Result.GetRawText(), JsonOptions);

         return result;
      }

      public async Task<TObject> Post<TObject>(string path, object obj)
      {

         //Console.WriteLine("Sending Post");
         var apiResponse = await Post(path, obj);
         //Console.WriteLine("Deserializing Post");
         var result = JsonSerializer.Deserialize<TObject>(apiResponse.Result.GetRawText(), JsonOptions);
         //Console.WriteLine("Post deserialized");
         return result;
      }

      public async Task<ApiResponse> Post(string path, object obj)
      {
         //Console.WriteLine("AcceptLanguage="+Http.DefaultRequestHeaders.AcceptLanguage.ToString());         
         var responseObj = await Http.PostAsJsonAsync(path, obj, JsonOptions);
         await ProcessResponse(responseObj);
         var apiResponse = await responseObj.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
         //TODO checar se a resposta foi ok, caso aconteça um erro reportar o erro
         return apiResponse;
      }

      public async Task<TObject> Put<TObject>(string path, object obj)
      {
         var apiResponse = await Put(path, obj);
         var result = JsonSerializer.Deserialize<TObject>(apiResponse.Result.GetRawText(), JsonOptions);
         return result;
      }

      public async Task<ApiResponse> Put(string path, object obj)
      {
         string json = JsonSerializer.Serialize(obj, JsonOptions);
         Console.WriteLine(json);

         var responseObj = await Http.PutAsJsonAsync(path, obj, JsonOptions);
         await ProcessResponse(responseObj);
         var apiResponse = await responseObj.Content.ReadFromJsonAsync<ApiResponse>();
         //TODO checar se a resposta foi ok, caso aconteça um erro reportar o erro
         return apiResponse;
      }

      public async Task<bool> Delete(string path, int id)
      {
         Console.WriteLine("Delete Path: " + path + " Id=" + id);
         var responseObj = await Http.DeleteAsync(path + $"?Id={id}");
         await ProcessResponse(responseObj);
         var apiResponse = await responseObj.Content.ReadFromJsonAsync<ApiResponse>();
         //TODO checar se a resposta foi ok, caso aconteça um erro reportar o erro
         return apiResponse.StatusCode == 200;

      }

      public async Task ProcessResponse(HttpResponseMessage response)
      {

         if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
         {
            JsonElement data = await response.Content.ReadFromJsonAsync<JsonElement>();
            var errors = data.GetProperty("errors").EnumerateObject().Select(x => x).ToList();

            string validationErrors = "<br>";
            foreach (var error in errors)
            {
               var details = error.Value.EnumerateArray().Select(x => x).ToList();
               validationErrors += error.Name + ": " + details.FirstOrDefault() + "<br>";
            }


            //Detalhar o erro de validação
            throw new Exception("Erros de validação: " + validationErrors);
         }
         else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
         {
            Console.WriteLine("Error: " + await response.Content.ReadAsStringAsync());
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();


            string message = null;
            try
            {
               JsonElement msg;
               apiResponse.Result.TryGetProperty("message", out msg);
               message = msg.GetString();
            }
            catch (Exception) { }

            if (message != null)
               throw new Exception(message);
            else
               throw new Exception("Ocorreu um erro desconhecido: " + apiResponse.Result.GetRawText().Replace(@"\r\n", "<br>"));
         }
         else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
         {
            Console.WriteLine("Error: " + await response.Content.ReadAsStringAsync());
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
            throw new Exception("O recurso acessado não foi encontrado: " + apiResponse.Result.GetRawText().Replace(@"\r\n", "<br>"));
         }
         else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
         {
            await AuthService.Logout();
            NavigateUnauthorized();
         }
         

         await RenewAuthorization();
      }

      public async Task RenewAuthorization()
      {
         try
         {
            if (AuthService.Token != null)
            {
               var diff = AuthService.Token.ValidTo - AuthService.Token.ValidFrom;
               var timeDiff = diff.Ticks * 30 / 100;

               //Console.WriteLine("Token Expiration: " + AuthService.Token.ValidTo.ToString());
               //Console.WriteLine("Token ValidFrom: " + AuthService.Token.ValidFrom.ToString());
               //Console.WriteLine("Renew When: " + AuthService.Token.ValidTo.AddTicks(timeDiff * -1));
               //Console.WriteLine("Time Now: " + DateTime.UtcNow);
               if (DateTime.UtcNow >= AuthService.Token.ValidTo.AddTicks(timeDiff * -1))
               {
                  //Esta chamada não deve ser awaited porque a aplicação não precisa esperar ela terminar

                  var responseObj = await Http.PostAsync(RenewPath, new StringContent(""));
                  var apiResponse = await responseObj.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions);
                  AuthService.Usuario = JsonSerializer.Deserialize<Daniel.ViewModels.UsuarioBaseViewModel>(apiResponse.Result.GetRawText(), JsonOptions);
                  AuthService.Usuario.Senha = null;

                  await AuthService.LocalStorageService.SetItemAsync("usuario", AuthService.Usuario);
                  Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthService.Usuario.Token);

               }
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine("Error ao renovar Token:" + ex.FullErrorMessage());
         }
      }

      public async Task<DataResult> ToDataResult(string action, DataManagerRequest dm)
      {
         var dataResult = await Post<DataResult>($"{action}", dm);
         return dataResult;
      }

      public List<TModel> ToListResult<TModel>(DataResult dataResult)
      {
         var elements = dataResult.Result.Cast<JsonElement>().Select(x => (x.JsonElementToType<TModel>(JsonOptions))).ToList();
         return elements;

      }

        public TModel ToResult<TModel>(DataResult dataResult)
        {
            var element = dataResult.Result.Cast<JsonElement>().Select(x => (x.JsonElementToType<TModel>(JsonOptions))).FirstOrDefault();
            return element;

        }

    }
}