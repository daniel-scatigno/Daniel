using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Daniel.Infra;
using AutoMapper;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Daniel.Web.Controllers
{
   public abstract class BaseController<TModel, TViewModel> : BaseApiController where TModel : class
   {
      public ClaimsPrincipal CurrentUser { get; set; }
      public int IdUsuario{get;set;}

      protected IMapper Mapeador { get; set; }
      protected virtual IUnitOfWork UOW { get; set; }
      public BaseController(IUnitOfWork uow, IMapper mapper)
      {
         var httpContext = new HttpContextAccessor().HttpContext;
         UOW = uow;
         Mapeador = mapper;
         CurrentUser = httpContext?.User;
         string idUsuario = CurrentUser?.FindFirstValue("IdUsuario");
         IdUsuario = string.IsNullOrEmpty(idUsuario)?0:int.Parse(idUsuario);
      }

      protected virtual TViewModel MapearParaViewModel(TModel model)
      {
         return Mapeador.Map<TModel, TViewModel>(model);
      }

      protected List<TViewModel> MapearParaViewModel(IEnumerable<TModel> model)
      {
         return model.Select(x => MapearParaViewModel(x)).ToList();
      }

      protected virtual TModel MapearParaModel(TViewModel model)
      {
         return Mapeador.Map<TViewModel, TModel>(model);
      }

      protected List<TModel> MapearParaModel(IEnumerable<TViewModel> model)
      {
         return model.Select(x => MapearParaModel(x)).ToList();
      }


      /// <summary>
      /// Antes de realizar o filtro, use este método para alterar a consulta padrão, ou adicionar os Includes
      /// </summary>
      /// <param name="query">retorna um IQueryable</param>
      /// <returns></returns>
      protected virtual IQueryable<TModel> AntesDeEnumerar(IQueryable<TModel> query)
      {
         return query;
      }

      protected virtual void AntesDeCriar(TModel model,TViewModel viewModel)
      {
      }

      protected virtual void DepoisDeCriar(TViewModel viewModel, TModel model)
      {
      }

      protected virtual void AntesDeAtualizar(TModel model,TViewModel viewModel)
      {
      }

      protected virtual void DepoisDeAtualizar(TViewModel viewModel, TModel model)
      {
      }

      protected virtual void AntesDeRemover(TModel model)
      {
      }

      protected virtual void DepoisDeRemover(TViewModel viewModel)
      {
      }



   }
}