using System.Threading;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NCPC.Resource.Resources;

namespace NCPC.Blazor.Components
{
   public partial class Listing<TViewModel> where TViewModel : class, new()
   {
      public Listing()
      {
      }

      SfSpinner SpinnerObj;

      [Parameter]
      public RenderFragment ChildContent { get; set; }
      
      [Parameter]
      public RenderFragment TopRightFragment{get;set;}

      [Parameter]
      public RenderFragment BeforeGrid { get; set; }
      [Parameter]
      public RenderFragment BeforeButtons { get; set; }

      [Parameter]
      public List<GridSortColumn> InitialSortColumns { get; set; } = new();

      [Parameter]
      public Syncfusion.Blazor.Data.Query Query{get;set;}

      [Parameter]
      public bool AllowGrouping{get;set;}

      [Parameter]
      public bool AllowFiltering{get;set;} =true;

      [Parameter]
      public bool AllowSorting{get;set;} = true;

      [Parameter]
      public int PageSize{get;set;} = 10;

      [Parameter]
      public bool AllowPaging{get;set;} = true;

      [Parameter]
      public bool HideNew{get;set;}

      [Parameter]
      public bool HideEdit{get;set;}
      
      [Parameter]
      public bool HideDelete{get;set;}


      public bool DisableEdit { get; set; } = true;

      public bool DisableDelete { get; set; } = true;

      [Parameter]
      public TViewModel TValue { get; set; }

      [Parameter]
      public Func<TViewModel, object> PrimaryKeyFunc { get; set; }

      [Parameter]
      public RenderFragment Columns { get; set; }

      public SfGrid<TViewModel> Grid { get; set; }

      public int Value { get; set; } = 1000;

      public TViewModel SelectedLine { get; set; }

      [Parameter]
      public EventCallback<TViewModel> AfterSelected { get; set; }

      [Parameter]
      public EventCallback<TViewModel> AfterCreate { get; set; }

      [Parameter]
      public EventCallback<TViewModel> AfterEdit { get; set; }

      [Parameter]
      public EventCallback<List<TViewModel>> AfterDataLoad { get; set; }

      [Parameter]
      public EventCallback AfterDelete { get; set; }

      [Parameter]
      public string ListAction { get; set; }

      [Parameter]
      public string DeleteAction { get; set; }

      [Parameter]
      public string FormPath { get; set; }

      public bool ShowDeleteDialog { get; set; }

      [Parameter]
      public List<TViewModel> DataSource { get; set; }

      [Parameter]
      public List<string> IncludeFields{get;set;}

      public DataAdaptorComponent<TViewModel> DataAdaptorComponent{get;set;}

      public async Task Teste()
      {
         //Necessário para não ocorrer um erro, isso porque a Tarefa OnSelectLine também é executada quando ocorre um click
         await Task.Yield();
         AppManager.ShowNotification("Registro salvo com sucesso!", null, NCPC.Blazor.Services.ToastType.Info);
      }

      public async Task OnSelectLine(RowSelectEventArgs<TViewModel> args)
      {
         await Task.Yield();
         SelectedLine = args.Data;
         DisableDelete = false;
         DisableEdit = false;
         await AfterSelected.InvokeAsync(SelectedLine);
      }

      public async Task OnRecordDoubleClick(RecordDoubleClickEventArgs<TViewModel> args)
      {
         //Necessário para não ocorrer um erro, isso porque a Tarefa OnSelectLine também é executada quando ocorre um click
         await Task.Yield();
         if (!HideEdit)
            await OnEditClick();
      }

      public async Task OnEditClick()
      {
         await Task.Yield();
         if (SelectedLine != null)
         {
            AppManager.PaginaViewModel = SelectedLine;
            if (!string.IsNullOrEmpty(FormPath))
               NavigationManager.NavigateTo($"{FormPath}/{PrimaryKeyFunc.Invoke(SelectedLine) }");
            await AfterEdit.InvokeAsync(SelectedLine);
         }
      }

      public async Task OnNewClick()
      {
         AppManager.PaginaViewModel = new TViewModel();
         if (!string.IsNullOrEmpty(FormPath))
            NavigationManager.NavigateTo($"{FormPath}/{0}");
         await AfterCreate.InvokeAsync((TViewModel)AppManager.PaginaViewModel);
      }

      public async Task OnDeleteClick()
      {
         await Task.Yield();
         ShowDeleteDialog = true;
      }

      public void DialogVisible()
      {
         ShowDeleteDialog = false;
      }

      private async Task DeleteRecord()
      {
         await Task.Yield();

         if (SelectedLine != null)
         {
            if (DataSource == null)
            {
               try
               {
                  bool deleted = await DataService.Delete($"{DeleteAction}", (int)PrimaryKeyFunc.Invoke(SelectedLine));
               }
               catch (Exception ex)
               {
                  AppManager.ShowNotification(ex.Message, null, Services.ToastType.Danger, 8000);
                  //Se ocorrer um erro, o metódo deve retornar pois o evento de ModelSaved não deve ser chamado
                  return;
               }               

               //TODO Localizar
               AppManager.ShowNotification(SfResources.RegistroRemovidoComSucesso, null, Services.ToastType.Info);
            }
            else
            {
               DataSource.Remove(SelectedLine);
            }
            ShowDeleteDialog = false;
            await AfterDelete.InvokeAsync();
            Grid.Refresh();

         }
      }
   }
}