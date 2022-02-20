using System.Threading;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Daniel.Blazor.Library;
using System.Collections;
using System.Linq;

namespace Daniel.Blazor.Components
{
   public partial class FormEditor<TViewModel> : IFormEditor, IDisposable where TViewModel : class, new()
   {
      private bool disableButtons = false;
      private bool disableSave = false;

      private bool disableCancel = false;

      [Parameter]
      public bool DisableButtons
      {
         get
         {
            return disableButtons;
         }
         set
         {
            if (value!=disableButtons)
            {
               disableButtons=value;
               Refresh();
            }

         }
      }

      [Parameter]
      public bool DisableSave
      {
         get
         {
            return disableSave;
         }
         set
         {
            if (value!=disableSave)
            {
               disableSave = value;
               Refresh();
            }

         }
      }

      [Parameter]
      public bool DisableCancel
      {
         get
         {
            return disableCancel;
         }
         set
         {
            if (value!=disableCancel)
            {
               disableCancel = value;
               Refresh();
            }

         }
      }

      public async Task Refresh()
      {
         await Task.Yield();
         StateHasChanged();
      }

      public void Dispose()
      {
         if (FormFather != null)
            FormFather.DisableButtons = false;
      }

      
    public async Task Submit(Microsoft.AspNetCore.Components.Forms.EditContext editContext)
    {
        await Task.Yield();

        TValue = (TViewModel)editContext.Model;
        await BeforeValidate.InvokeAsync(TValue);
        var errorEvent = new CustomErrorEventArgs<TViewModel>() { Model = TValue, CustomErrors = new Dictionary<string, string>() };


        await CustomErrorValidation.InvokeAsync(errorEvent);
        CustomErrors = errorEvent.CustomErrors;
        bool isValid = editContext.Validate() && errorEvent.CustomErrors.Count == 0;
        ValidationMessages = editContext.GetValidationMessages().ToList().Concat(CustomErrors.Select(x => x.Value.ToString())).ToList();
        if (isValid)
        {
            //Se um controller não foi informado, o form é statico (Sem execução da API)
            if (Controller != null)
            {
                //BeforeSave deve vir depois da validação, em um momento em que é certeza que o modelo será salvo

                try
                {
                    await BeforeSave.InvokeAsync(TValue);
                    if (IsEditing && UpdateMethod == "")
                        TValue = await DataService.Put<TViewModel>($"{Controller}/Atualizar", TValue);

                    else if (IsEditing && UpdateMethod != "")
                    {
                        TValue = await DataService.Put<TViewModel>($"{Controller}/{UpdateMethod}", TValue);
                    }
                    else if (CreateMethod != "")
                    {
                        TValue = await DataService.Post<TViewModel>($"{Controller}/{CreateMethod}", TValue);
                    }
                    else
                    {
                        TValue = await DataService.Post<TViewModel>($"{Controller}/Criar", TValue);
                    }

                }
                catch (Exception ex)
                {
                    AppManager.ShowNotification(ex.Message, null, Services.ToastType.Danger, 8000);
                    //Se ocorrer um erro, o metódo deve retornar pois o evento de ModelSaved não deve ser chamado
                    return;
                }
                //TODO Localizar
                AppManager.ShowNotification("Registro salvo com sucesso", null, Services.ToastType.Info);
            }
            await AfterSave.InvokeAsync(TValue);

        }
        else
        {
            //Console.Write("OCorreram erros");
            AppManager.ShowNotification($"O formulário tem erros:<br>"+string.Join("<br>",ValidationMessages), null, Services.ToastType.Danger, 8000);
        }


    }


   }
}
