using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Threading;
using Daniel.ViewModels;
using System.Net.Http;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace Daniel.Blazor.Services
{
   public enum ToastType { Info, Success, Warning, Danger };
   public delegate void NotifyState();

   public delegate void Notification(string content, string title, ToastType toastType = ToastType.Info, int timeout = 5000);




   public class AppManager
   {

      public HttpClient Client { get; set; }

      public ILocalStorageService LocalStorage { get; set; }

      private IUsuarioViewModel UsuarioViewModel { get; set; }

      public AppManager(ILocalStorageService localStorage, HttpClient client)
      {
         LocalStorage = localStorage;
         Client = client;
      }

      public object PaginaViewModel { get; set; }
      public event Notification OnNotificationRised;


      public async Task<string> GetAccessToken()
      {

         //Adicionar um m�todo para buscar o AccessToken do LocalStorage
         //ver o site https://www.puresourcecode.com/dotnet/blazor/use-localstorage-with-blazor-webassembly/
         string token = await LocalStorage.GetItemAsStringAsync("Token");
         return token;
      }


      public void ShowNotification(string content, string title, ToastType toastType = ToastType.Info, int timeout = 5000)
      {
         OnNotificationRised.Invoke(content, title, toastType, timeout);

      }

   }
}
