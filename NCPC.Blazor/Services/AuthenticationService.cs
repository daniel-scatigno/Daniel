using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using NCPC.ViewModels;
using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using NCPC.Utils;
using System.Text.Json;


namespace NCPC.Blazor.Services
{
   public class AuthenticationService
   {
      private NavigationManager NavigationManager;
      public ILocalStorageService LocalStorageService;

      public JwtSecurityToken Token { get; set; }

      private IUsuarioViewModel _Usuario;
      public IUsuarioViewModel Usuario
      {
         get
         {
            if (_Usuario != null)
            {
               if (Token!=null && DateTime.Now > Token.ValidTo)
               {
                  LocalStorageService.RemoveItemAsync("usuario");
                  return null;
               }
            }
            return _Usuario;
         }
         set
         {
            _Usuario = value;
            if (_Usuario != null)
            {
               var token = DecodeToken(_Usuario.Token);

               if (DateTime.Now > token.ValidTo)
               {
                  LocalStorageService.RemoveItemAsync("usuario");                  
                  Token=null;
               }
               Token = token;

            }

         }
      }

      public HttpClient Client { get; set; }

      public AuthenticationService(
          HttpClient httpClient,
          NavigationManager navigationManager,
          ILocalStorageService localStorageService
      )
      {
         Client = httpClient;
         NavigationManager = navigationManager;
         LocalStorageService = localStorageService;
         
      }

      public async Task Initialize()
      {
         Console.WriteLine("Authorization Initialize");
         Usuario = await LocalStorageService.GetItemAsync<UsuarioBaseViewModel>("usuario");
         Console.WriteLine("Usuario:" + Usuario?.NomeUsuario);
         Console.WriteLine("Usuario Permissoes:" + Usuario?.PermissoesDoUsuario);
         if (Usuario != null)
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Usuario.Token);
      }

      public async Task Login(LoginViewModel login, string path,DataService ds)
      {
         Usuario = await ds.Post<UsuarioBaseViewModel>(path, login);
         Usuario.Senha = null;
         await LocalStorageService.SetItemAsync("usuario", Usuario);
         await LocalStorageService.SetItemAsStringAsync("BlazorCulture", login.SiglaIdioma);
         Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Usuario.Token);

      }

      public async Task Logout()
      {
         Usuario = null;
         await LocalStorageService.RemoveItemAsync("usuario");
         //Manter a seleção da linguagem
         //await LocalStorageService.RemoveItemAsync("BlazorCulture"); 
      }

      public JwtSecurityToken DecodeToken(string token)
      {
         var stream = token;
         var handler = new JwtSecurityTokenHandler();
         var jsonToken = handler.ReadToken(stream);
         var tokenS = jsonToken as JwtSecurityToken;
         return tokenS;

      }

      public async Task<string> GetUserLanguage()
      {
         string lang = await LocalStorageService.GetItemAsStringAsync("BlazorCulture");
         return string.IsNullOrEmpty(lang)?"pt-BR":lang;
      }

      // private async Task GetClaimsPrincipalData()
      // {
      //    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
      //    var user = authState.User;
      // }
   }
}