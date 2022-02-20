using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Threading;
using NCPC.ViewModels;
using NCPC.Utils;
using System.Net.Http;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;


namespace NCPC.Blazor.Services
{



   public class NotificationService<TNotification> where TNotification : NotificacaoBaseViewModel
   {
      public delegate void NotificationBadge(List<TNotification> notificacao);
      protected string Path { get; set; }

      protected DataService DataService { get; set; }
      protected AuthenticationService AuthService { get; set; }

      public List<TNotification> Notifications { get; set; }

      public event NotificationBadge OnNotificationRised;

      private int LastId { get; set; }
      private int TakeCount { get; set; } = 10;
      public int TotalCount { get; set; }
      private System.Threading.Timer Timer { get; set; }

      public NotificationService(DataService dataService, AuthenticationService authService, string path)
      {
         DataService = dataService;
         AuthService = authService;
         Path = path;
         Notifications = new();
         Timer = new System.Threading.Timer(FindNewNotifications, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30));

      }

      public void FindNewNotifications(object obj)
      {
         try
         {
            if (AuthService.Usuario != null)
            {
               var dm = new DataManagerRequest()
               {
                  Where = new List<WhereFilter>(){new WhereFilter(){Field="Id",Operator="greaterThan",value=LastId}},
                  Take = TakeCount,
                  Sorted = new List<Sort>() { new Sort() { Name = "DataCriacao", Direction = "descending" } }
               };

               DataService.ToDataResult(Path, dm).ContinueWith((t) =>
               {
                  /*Soma o total count pois: a primeira vez traz o total de registros, nas subsequentes consultas 
                     o total é apenas de registros novos
                  */
                  TotalCount += t.Result.Count;
                  Console.WriteLine("TotalCount:"+TotalCount);
                  var lstResult = DataService.ToListResult<TNotification>(t.Result);
                  Notifications.AddRange(lstResult.Where(x => !Notifications.Any(a => a.Id == x.Id)));
                  Notifications = Notifications.OrderByDescending(x => x.DataCriacao).ToList();
                  if (Notifications.Any())
                     LastId = Notifications.Select(x => x.Id).Max();
                  if (lstResult.Count > 0)
                  {
                     OnNotificationRised?.Invoke(lstResult);
                  }
               });
            }
         }
         catch (Exception ex)
         {
            //TODO Log de erros
            Console.WriteLine("Erro ao buscar novas notificações: " + ex.FullErrorMessage());

         }

      }

      public async Task LoadMore()
      {
           try
         {
            if (AuthService.Usuario != null && Notifications.Any())
            {
               var oldestId = Notifications.OrderBy(x=>x.Id).FirstOrDefault().Id;
               var dm = new DataManagerRequest()
               {
                  Where = new List<WhereFilter>(){new WhereFilter(){Field="Id",Operator="lessThan",value=oldestId}},
                  Take = TakeCount,
                  Sorted = new List<Sort>() { new Sort() { Name = "DataCriacao", Direction = "descending" } }
               };

               var result = await DataService.ToDataResult(Path, dm);
                              
               var lstResult = DataService.ToListResult<TNotification>(result);
               Notifications.AddRange(lstResult.Where(x => !Notifications.Any(a => a.Id == x.Id)));
               Notifications = Notifications.OrderByDescending(x => x.DataCriacao).ToList();
               
            }
         }
         catch (Exception ex)
         {
            //TODO Log de erros
            Console.WriteLine("Erro ao buscar novas notificações: " + ex.FullErrorMessage());

         }

      }

      public async Task GetPastNotifications(int lastId)
      {

         var dm = new DataManagerRequest()
         {
            Where = new List<WhereFilter>(){
               new WhereFilter(){Field="IdUsuario",Operator="equals",value=AuthService.Usuario.Id}.And("Id","lessThan",lastId)
            },
            Take = TakeCount,
            Sorted = new List<Sort>() { new Sort() { Name = "DataCriacao", Direction = "ascending" } }
         };

         var dataResult = await DataService.ToDataResult(Path, dm);
         TotalCount = dataResult.Count;
         var lstResult = DataService.ToListResult<TNotification>(dataResult);
         Notifications.AddRange(lstResult.Where(x => !Notifications.Any(a => a.Id == x.Id)));
         Notifications = Notifications.OrderBy(x => x.DataCriacao).ToList();
         if (lstResult.Count > 0)
            OnNotificationRised?.Invoke(lstResult);

      }
   }
}