using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Daniel.Infra
{
   public interface IUnitOfWork : IDisposable
   {      
      int SaveChanges();
      TRepo GetRepository<TRepo>() where TRepo : class;

      void StartTransaction();
      
      void CommitTransaction();
      

      void RollbackTransaction();

      IEnumerable<EntityEntry> GetChanges(Func<EntityEntry,bool> func);
      List<string> GetUpdateChangesLog(EntityEntry entry);
      
   }
}