using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Daniel.Web.Library
{

   public class RequirePermissionAttribute : TypeFilterAttribute
   {
      public int Permission{get;set;}
      public RequirePermissionAttribute(object permission) : base(typeof(AuthorizeActionFilter))
      {
         Permission = (int)permission;
         Arguments = new object[] { (int)permission };
      }
   }

   public class AuthorizeActionFilter : IAuthorizationFilter
   {
      private readonly int _item;
      
      public AuthorizeActionFilter(int item)
      {
         _item = item;
      }
      public void OnAuthorization(AuthorizationFilterContext context)
      {
         
         bool isAuthorized = context.HttpContext.User.HasClaim(x=>x.Type=="Permission" && x.Value==_item.ToString());

         if (!isAuthorized)
         {
            context.Result = new ForbidResult();
         }
      }
   }

   
}