using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using NCPC.Infra;
using NCPC.ViewModels;
using NCPC.Utils;
using AutoMapper;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Cors;
using System.Linq.Dynamic.Core;


namespace NCPC.Web.Controllers
{
   public abstract class ApiController<TModel, TViewModel> : BaseController<TModel, TViewModel> where TModel : class
   {
      protected IRepository<TModel> Repo { get; set; }
      public Expression<System.Func<TModel, bool>> DefaultFilter { get; set; }
      
      //Nome do campo usado como filtro (Usado no DropDown)
      public string FilterField = "Descricao";
      //Nome do campo usado como descrição (Usado no DropDown)
      public string ShownField = "Descricao";

      public ApiController(IUnitOfWork uow, IRepository<TModel> repo, IMapper mapper) : base(uow, mapper)
      {
         Repo = repo;
      }

      /// <summary>
      /// Realiza os filtros e ordenação
      /// </summary>
      /// <param name="dm">DataManagerRequest utilizado nos componentes Syncfusion</param>
      /// <returns></returns>
      [NonAction]
      public virtual IQueryable<TObject> PerformFilter<TObject>(DataManagerRequest dm, IQueryable<TObject> queryable) where TObject : class
      {
         if (dm.Sorted != null && dm.Sorted.Any())
         {
            for (int i = 0; i < dm.Sorted.Count; i++)
            {
               //Usando o  Dynamic Linq para ordenar a consulta
               string direction = (dm.Sorted[i].Direction == "descending" ? " desc" : " asc");
               if (i == 0)
                  queryable = queryable.OrderBy(dm.Sorted[i].Name + direction);
               else
                  queryable = ((IOrderedQueryable<TObject>)queryable).ThenBy(dm.Sorted[i].Name + direction);

            }
            //registros = DataOperations.PerformSorting(registros, dm.Sorted);
         }
         if (dm.Where != null && dm.Where.Any())
            queryable = DataOperations.PerformFiltering(queryable, dm.Where, "AND");

         return queryable;

      }


      /// <summary>
      /// Obtém um objeto DataResult utilizado nos componentes SyncFusion, 
      /// </summary>
      /// <typeparam name="TObject"></typeparam>
      [NonAction]
      public virtual IQueryable<TObject> PerformTakeSkip<TObject>(DataManagerRequest dm, IQueryable<TObject> queryable) where TObject : class
      {
         if (dm.Skip > 0)
            queryable = DataOperations.PerformSkip(queryable, dm.Skip);
         if (dm.Take > 0)
            queryable = DataOperations.PerformTake(queryable, dm.Take);

         return queryable;

      }


      [HttpPost]
      [Route("[controller]/Listar")]
      public virtual ActionResult Listar(DataManagerRequest dm)
      {
         //O Agrupamento quando existir, não é feito aqui, e sim no Frontend(Client) => DataAdaptorComponent.razor.cs
         //Por isso quando um consulta é agrupada no grid do SyncFusion, o campo Sorted é preenchido com as colunas agrupadas 

         return Ok(Listar(dm, Repo.GetAll()));
      }

      [NonAction]
      public virtual DataResult Listar(DataManagerRequest dm, IQueryable<TModel> query)
      {
         //Filtra os registros de acordo com o DataManagerRequest
         var registros = PerformFilter<TModel>(dm, query);

         //Documentação Syncfusion
         //https://help.syncfusion.com/cr/blazor/Syncfusion.Blazor.DataOperations.html#Syncfusion_Blazor_DataOperations_PerformFiltering_System_Collections_IEnumerable_System_Collections_Generic_List_Syncfusion_Blazor_Data_WhereFilter__System_String_
         //var dataResult = GetDataResult(dm);
         DataResult dataResult = new DataResult();
         if (DefaultFilter != null)
            registros = registros.Where(DefaultFilter);

         dataResult.Count = registros.Count();
         registros = PerformTakeSkip(dm, registros);
         if (dm.Select != null)
         {
            foreach (string s in dm.Select)
               registros = registros.Include(s);
         }

         var viewModels = MapearParaViewModel(AntesDeEnumerar(registros));
         dataResult.Result = viewModels;
         return dataResult;
      }

      [HttpPost]
      [Route("[controller]/ListarAtivo")]
      public virtual ActionResult ListarAtivo(DataManagerRequest dm)
      {
         if (dm.Where == null)
            dm.Where = new List<WhereFilter>();
         dm.Where.Add(new WhereFilter() { Field = "Ativo", Operator = "equals", value = true });
         return Listar(dm);

      }

      [HttpPost]
      [Route("[controller]/ListarDropDown")]
      public ActionResult ListarDropDown(DataManagerRequest dm)
      {
         if (dm.Where != null)
            foreach (var w in dm.Where)
               if (w.Field == "Descricao")
                  w.Field = FilterField;

         var dataResult = Listar(dm, Repo.GetAll());
         dataResult.Result = ((List<TViewModel>)dataResult.Result).Select(x => new DropDownViewModel()
         {
            Id = (int)typeof(TViewModel).GetProperty("Id").GetValue(x),
            Descricao = (string)typeof(TViewModel).GetProperty(ShownField).GetValue(x)
         });
         return Ok(dataResult);
      }

      [HttpGet]
      [Route("[controller]/Obter")]
      public virtual ActionResult Obter(int id)
      {
         var registro = Obter(id, null);
         if (registro == null)
            return NotFound();
         return Ok(registro);
      }

      [NonAction]
      public virtual TViewModel Obter(int id, List<string> includeFields)
      {
         var registros = Repo.GetQueryableById(id);
         if (DefaultFilter != null)
            registros = registros.Where(DefaultFilter);

         if (includeFields != null)
         {
            foreach (string s in includeFields)
               registros = registros.Include(s);
         }

         var registro = AntesDeEnumerar(registros).FirstOrDefault();
         if (registro == null)
            return default(TViewModel);

         var viewModel = MapearParaViewModel(registro);

         return viewModel;
      }


      [HttpPost]
      [Route("[controller]/Criar")]
      public virtual ActionResult Criar(TViewModel viewModel)
      {
         //Para validação customizada, veja o UsuarioViewModel, a validação é feita na ViewModel
         var model = MapearParaModel(viewModel);

         //Evento antes da criação do objeto no banco, este método pode ser sobreposto no controller instanciado

         AntesDeCriar(model, viewModel);


         //Adiciona o Modelo no banco de dados (sem commit)         
         Repo.Add(model);

         //Executa um commita das mudanças
         UOW.SaveChanges();

         //Executa o evento depois de criar o objeto, este método pode ser sobreposto no controller instanciado
         DepoisDeCriar(viewModel, model);

         //Mapea de volta para viewModel pois o Id do objeto foi gerado
         viewModel = MapearParaViewModel(model);



         return Ok(viewModel);
      }

      [HttpPut]
      [Route("[controller]/Atualizar")]
      public virtual ActionResult Atualizar(TViewModel viewModel)
      {
         var model = MapearParaModel(viewModel);

         AntesDeAtualizar(model, viewModel);

         //Atualiza o modelo, mesmo tendo sido convertido da viewModel o EntityFramework usa o Id para atualizar
         Repo.Update(model);
         UOW.SaveChanges();

         DepoisDeAtualizar(viewModel, model);
         viewModel = MapearParaViewModel(model);

         return Ok(viewModel);
      }

      [HttpDelete]
      [Route("[controller]/Remover")]
      public virtual ActionResult Remover(TViewModel viewModel)
      {
         var model = MapearParaModel(viewModel);
         AntesDeRemover(model);
         Repo.Delete(model);
         UOW.SaveChanges();
         DepoisDeRemover(viewModel);

         return Ok(viewModel);
      }

      [HttpDelete]
      [Route("[controller]/RemoverPorId")]
      public virtual ActionResult RemoverPorId(int id)
      {
         //Busca o objeto pelo id para exclusão, caso não exista uma exception ocorre
         var model = Repo.GetById(id);
         var viewModel = MapearParaViewModel(model);

         AntesDeRemover(model);
         Repo.Delete(model);
         UOW.SaveChanges();
         DepoisDeRemover(viewModel);

         return Ok(viewModel);
      }
   }
}