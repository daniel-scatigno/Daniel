using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using Daniel.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Daniel.Infra
{
   public class UnitOfWork : IUnitOfWork
   {
      private DbContext Context { get; set; }
      private IDbContextTransaction Transaction { get; set; }

      private ClaimsPrincipal Principal { get; set; }

      private int IdUsuario { get; set; }
      private string Login { get; set; }
      public UnitOfWork(DbContext context)
      {
         Context = context;
      }
      public UnitOfWork(DbContext context, ClaimsPrincipal principal) : this(context)
      {
         Principal = principal;

         if (Principal != null)
         {
            int i = 0;
            int.TryParse(Principal?.FindFirstValue("IdUsuario"), out i);
            if (i > 0)
               IdUsuario = i;
            Login = Principal.Identity.Name;
         }
      }

      public virtual IEnumerable<EntityEntry> GetChanges(Func<EntityEntry, bool> func)
      {
         return Context.ChangeTracker.Entries().Where(func);
      }

      public virtual List<string> GetUpdateChangesLog(EntityEntry entry)
      {
         List<string> changes = new List<string>();
         var oldObj = entry.GetDatabaseValues().ToObject();
         var newObj = entry.CurrentValues.ToObject();

         foreach (PropertyInfo property in entry.OriginalValues.Properties.Select(x => x.PropertyInfo))
         {
            object original = property.GetValue(oldObj);
            object current = property.GetValue(newObj);

            if ( !object.Equals(original, current))
               changes.Add($"({property.Name}) {original} => {current}");
         }
         return changes;
      }

      public virtual int SaveChanges()
      {
         List<ActionLog> actions = new List<ActionLog>();

         foreach (var entry in Context.ChangeTracker.Entries().Where
            (x => (x.State == EntityState.Added) ||
                (x.State == EntityState.Deleted) ||
                (x.State == EntityState.Modified)))
         {

            var al = new ActionLog()
            {
               IdUser = IdUsuario,
               Login = Login,
               Date = DateTime.Now,
               ObjectType = entry.Entity.GetType().Name,
               Entity = entry.Entity
            };

            switch (entry.State)
            {
               case EntityState.Added: al.Action = ActionType.Add; break;
               case EntityState.Deleted: al.Action = ActionType.Delete; break;
               case EntityState.Modified: al.Action = ActionType.Update; break;
               default: break;
            }

            //var t = Context.ChangeTracker.ToDebugString(Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTrackerDebugStringOptions.LongDefault);
            //var t2 = Context.ChangeTracker.ToDebugString(Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTrackerDebugStringOptions.ShortDefault);

            if (al.Action == ActionType.Update)
            {
               al.Change = string.Join(";", GetUpdateChangesLog(entry));
            }
            
            actions.Add(al);
         }
         int result = Context.SaveChanges();

         //A geração de logs não pode dar exception na ação principal
         try
         {
            actions.ForEach(a =>
            {
               a.IdRecord = (int)Context.Model.FindEntityType(
                     a.Entity.GetType()).FindPrimaryKey().Properties.FirstOrDefault().PropertyInfo.GetValue(a.Entity);
            });

            Context.AddRange(actions);
            Context.SaveChanges();//É necessário esperar pois o Dispose da um erro 
         }
         catch (Exception ex)
         {
         }

         return result;
      }

      public virtual void Dispose()
      {
         Context.Dispose();
      }


      public TRepo GetRepository<TRepo>() where TRepo : class
      {
         TRepo repo = null;
         repo = (TRepo)Activator.CreateInstance(typeof(TRepo), Context);

         return repo;
      }

      public void StartTransaction()
      {
         Transaction = Context.Database.BeginTransaction();
      }
      public void CommitTransaction()
      {
         Transaction.Commit();
      }

      public void RollbackTransaction()
      {
         if (Transaction != null)
            Transaction.Rollback();
      }
   }
}