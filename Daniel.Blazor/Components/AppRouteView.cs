
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;
using System.Net;
using Daniel.Blazor.Services;
using Daniel.Utils;


//https://jasonwatmore.com/post/2020/08/13/blazor-webassembly-jwt-authentication-example-tutorial#app-route-view-cs
namespace Daniel.Blazor.Components
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationService AuthService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
           
            var authorizeAttribute = (AuthorizeAttribute)RouteData.PageType.GetCustomAttributes(typeof(AuthorizeAttribute),false).
               Where(x=>x.GetType()!=typeof(AuthorizePermissionAttribute)).FirstOrDefault();
            var authorizePolicyAttribute = (AuthorizePermissionAttribute)RouteData.PageType.GetCustomAttributes(typeof(AuthorizePermissionAttribute),false).FirstOrDefault();
            

            //Caso o usuário não esteja logado, redireciona para página de login
            if ( (authorizeAttribute != null || authorizePolicyAttribute!=null) && AuthService.Usuario == null)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                Console.WriteLine("ReturnUrl:" + returnUrl);

                NavigationManager.NavigateTo($"login?returnUrl={returnUrl}", true);
            }
            //Verifica se existe o atributo authorize  e se o usuário tem o Papel necessário ou a politica necessária
            else if ( ( 
                  authorizeAttribute!=null && !string.IsNullOrEmpty(authorizeAttribute.Roles)
                  &&!IsUserAllowed(authorizeAttribute.Roles, AuthService.Usuario.PapeisDoUsuario)                  
                )
               || (
                   authorizePolicyAttribute!=null && !IsUserAllowed(authorizePolicyAttribute.Permission, AuthService.Usuario.PermissoesDoUsuario)  )
                  )
            {
                NavigationManager.NavigateTo($"NotAuthorized", true);
            }
            else
            {

                base.Render(builder);
            }
        }

        protected virtual bool IsUserAllowed(string roles,string userRoles)
        {
           if (roles == null || userRoles == null)
            return false;
            var rolesList = roles.Split(",").Select(x => x.ToLower()).ToList();
            var userRolesList = userRoles.Split(";").Select(x => x.ToLower()).ToList();

            
            return rolesList.Any(a => userRolesList.Contains(a));
        }
    }
}