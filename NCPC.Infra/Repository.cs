using System.Threading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NCPC.Domain.Models;

namespace NCPC.Infra
{
   public abstract class Repository<T> : IRepository<T> where T : class
   {
      protected DbContext Context { get; set; }
      protected DbSet<T> DbSet { get; set; }

      private IQueryable<T> DefaultQuery { get; set; }

      private ILogger<Repository<T>> Log { get; set; }


      private List<Expression<Func<T, object>>> IncludeQueries { get; set; }

      private int IdUsuario { get; set; }



      public Repository(DbContext context, int? idUsuario = null)
      {
         Context = context;
         DbSet = context.Set<T>();
         DefaultQuery = DbSet.AsQueryable<T>();
         if (idUsuario.HasValue)
            IdUsuario = idUsuario.Value;
      }
      public Repository(DbContext context, ILogger<Repository<T>> log, int? idUsuario = null) : this(context, idUsuario)
      {
         Log = log;
      }

      public Exception ProcessException(Exception ex)
      {
         //Log das exceções, ou tratamento das mensagens de erro a serem enviadas ao usuário
         if (Log != null)
            Log.LogError(ex, "Exception on Repository: " + typeof(T).Name);

         return ex;
      }

      #region Métodos de Leitura

      public virtual IQueryable<T> GetQueryableById(int id, bool noTracking = false)
      {
         try
         {
            var parameterExpression = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameterExpression, Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault().PropertyInfo);
            var constant = Expression.Constant(id);
            var expression = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameterExpression);

            IQueryable<T> result;

            if (noTracking)
               result = DefaultQuery.Where(lambda).AsNoTracking();
            else
               result = DefaultQuery.Where(lambda);

            return result;
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      public virtual T GetById(int id, bool noTracking = false)
      {
         try
         {
            return GetQueryableById(id, noTracking).FirstOrDefault();
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      public virtual T GetFirst(System.Linq.Expressions.Expression<System.Func<T, bool>> expression, bool noTracking)
      {
         try
         {
            T result;
            if (noTracking)
               result = DefaultQuery.AsNoTracking().FirstOrDefault(expression);
            else
               result = DefaultQuery.FirstOrDefault(expression);
            return result;
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);

         }
         finally
         {

         }
      }

      public virtual IQueryable<T> GetAll()
      {
         try
         {
            return DefaultQuery;
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      public virtual IQueryable<T> GetBy(System.Linq.Expressions.Expression<System.Func<T, bool>> expression)
      {
         try
         {
            IQueryable<T> query = DefaultQuery.Where(expression);
            return query;
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      /// <summary>
      /// Executa um comando SQL para retornar dados
      /// A query deve retornar dados somente do tipo T
      /// Para trazer os dados de outros objetos use o método Include
      /// Exemplo "EXECUTE dbo.GetMostPopularBlogsForUser @filterByUser=@user"
      /// A Ordem dos parametros deve ser a mesma ordem dos parametros da query
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="values"></param>
      /// <returns></returns>
      public virtual IQueryable<T> GetByRawSql(string sql, params object[] parameters)
      {
         try
         {
            var q = DbSet.FromSqlRaw(sql, parameters);

            return q;
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      /// <summary>
      /// Obtém uma lista de objetos (Linhas) usando um comando SQL
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="values"></param>
      /// <returns></returns>
      public virtual List<object> GetUnknowByRawSql(string sql, Dictionary<string, object> values)
      {
         List<object> lst = new List<object>();
         try
         {
            using (var context = Context)
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
               command.CommandText = "SELECT * From Table1";
               context.Database.OpenConnection();
               using (var result = command.ExecuteReader())
               {
                  //É preciso navegar no datareater e converter ele para um datatable ou outro objeto navegavel como uma List<Dictionary<string,object>>
                  throw new Exception("Não implementado");
                  // do something with result
               }
            }
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      /// <summary>
      /// Retorna um único resultado para um comando SQL
      /// </summary>
      /// <param name="sql">The sql query to execute</param>
      /// <param name="values">Dictionary values, use the name parameter and value. The named parameter should be contained in the SQL query as a generic parameter like :namedparameter</param>
      /// <returns>object that represents unique result (integer, string, date, etc...)</returns>
      public virtual object GetUniqueResultByRawSql(string sql, Dictionary<string, object> values)
      {

         try
         {
            using (var context = Context)
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
               command.CommandText = "SELECT * From Table1";
               context.Database.OpenConnection();
               return command.ExecuteScalar();
            }

         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }

      }

      #endregion

      #region Métodos que alteram dados

      public virtual bool Add(T entity)
      {

         try
         {
            DbSet.Add(entity);
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);

         }
         return true;
      }

      public virtual bool Add(IEnumerable<T> lst)
      {
         try
         {
            DbSet.AddRange(lst);
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
         return true;
      }

      public virtual bool Update(T entity)
      {
         try
         {
            DbSet.Update(entity);            
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
         return true;
      }

      public virtual bool Update(IEnumerable<T> lst)
      {
         try
         {
            DbSet.UpdateRange(lst);
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
         return true;
      }


      /// <summary>
      /// Excluí os registros utilizando um comando SQL e retorna a quantidade de registros excluídos
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="values"></param>
      /// <returns></returns>
      public virtual int DeleteByRawSql(string sql, Dictionary<string, object> values)
      {
         try
         {

            using (var context = Context)
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
               //TODO é necessário adaptar este comando para o SQL Server
               command.CommandText = "with deleted AS (" + sql.TrimEnd(';') + " RETURNING *) select count(*) from deleted;";
               context.Database.OpenConnection();
               return (int)command.ExecuteScalar();
            }            
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      /// <summary>
      /// Execute command
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="values"></param>
      /// <param name="timeOut">0 will use the default Timeout Connection, Timeout value in seconds</param>
      /// <returns></returns>
      public virtual int ExecuteByRawSql(string sql, Dictionary<string, object> values, int timeOut = 0)
      {
         try
         {
            using (var context = Context)
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
               command.CommandText = sql;
               context.Database.OpenConnection();
               command.CommandTimeout = timeOut;
               var result = command.ExecuteNonQuery();
               return result;
            }
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
      }

      public bool Delete(int Id)
      {
         return Delete(GetById(Id));
      }

      public bool Delete(T entity)
      {

         try
         {
            DbSet.Remove(entity);
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
         return true;
      }

      public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
      {
         try
         {
            DbSet.RemoveRange(entities);
         }
         catch (Exception ex)
         {
            throw ProcessException(ex);
         }
         return true;
      }
      #endregion

      public void Attatch(T obj)
      {
         Context.Attach<T>(obj);
      }
      public void Attatch<TType>(TType obj) where TType : class
      {
         Context.Attach<TType>(obj);
      }

      public void SetAsModified(T obj)
      {
         Context.Entry(obj).State = EntityState.Modified;
      }

   }

}
