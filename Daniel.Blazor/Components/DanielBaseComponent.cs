using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Daniel.Blazor.Services;
using System.Net.Http;
using Blazored.LocalStorage;
namespace Daniel.Blazor.Components
{
   public class DanielBaseComponent : ComponentBase
   {
      [Inject]
      public AppManager AppManager { get; set; }


      [Inject]
      public AuthenticationService AuthService { get; set; }

      [Inject]
      public ILocalStorageService LocalStorageService {get;set;}

      [Inject]
      public NavigationManager NavigationManager { get; set; }

      [Inject]
      public DataService DataService { get; set; }

      [Inject]
      public HttpClient Http { get; set; }

      [Inject]
      protected IJSRuntime JsRuntime { get; set; }

      

      [Parameter]
      public IFormEditor FormFather { get; set; }



   }
}