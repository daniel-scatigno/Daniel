using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NCPC.Blazor.Services;
using System.Net.Http;
using Blazored.LocalStorage;
namespace NCPC.Blazor.Components
{
   public class NCPCBaseComponent : ComponentBase
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