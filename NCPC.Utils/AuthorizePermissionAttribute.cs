using Microsoft.AspNetCore.Authorization;
using System;

namespace NCPC.Utils
{
   
   public class AuthorizePermissionAttribute : AuthorizeAttribute 
   {
      public string Permission{get;set;}
      
      public AuthorizePermissionAttribute(object permission) 
      {
         Permission = permission.ToString();
      }
      
   }
}