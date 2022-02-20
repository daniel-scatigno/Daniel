using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using NCPC.Blazor.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using NCPC.Utils;
namespace NCPC.Blazor.Library
{
   public static class BlazorExtension
   {
      public static void AddValidationMessageFor<TTYpe>(this RenderTreeBuilder builder, PropertyInfo prop, object value)
      {

         builder.OpenComponent<ValidationMessage<TTYpe>>(2);
         var parameter = Expression.Constant(value);
         var body = Expression.MakeMemberAccess(parameter, prop);
         var resultToReturn = Expression.Lambda(body);

         builder.AddAttribute(0, "For", resultToReturn);
         builder.CloseComponent();
      }

       public static void AddValidationMessageFor<TType>(this RenderTreeBuilder builder, Expression<Func<TType>> func) 
      {
         builder.OpenComponent<ValidationMessage<TType>>(2);
         builder.AddAttribute(0, "For", func);
         builder.CloseComponent();
      }

      public static void AddLabel(this RenderTreeBuilder builder, PropertyInfo prop, int sequence,string requiredSymbol = "*")
      {
         string desc =  prop.GetDisplayName()+(prop.IsRequired()?" "+requiredSymbol:"");
         AddLabel(builder,desc,sequence);
      }

      public static void AddLabel(this RenderTreeBuilder builder, string value, int sequence)
      {         
         builder.OpenElement(sequence, "Label");
         builder.AddContent(0, value);
         builder.CloseElement();
      }

      public static RenderFragment InsertIntoColumn(this RenderFragment fragment, string cssClass)
      {
         return fragment.InsertIntoDiv("form-group " + cssClass);
      }

      public static RenderFragment InsertIntoDiv(this RenderFragment fragment, string cssClass)
      {
         RenderFragment renderFragment = (builder) =>
        {
           builder.OpenElement(0, "div");
           builder.AddAttribute(0, "class", cssClass);
           builder.AddContent(1, fragment);
           builder.CloseComponent();

        };
         return renderFragment;
      }

      public static RenderFragment Append(this RenderFragment fragment, RenderFragment additionalFragment)
      {
         RenderFragment renderFragment = (builder) =>
         {
            builder.AddContent(0, fragment);
            builder.AddContent(1, additionalFragment);
         };
         return renderFragment;
      }
      public static RenderFragment Prepend(this RenderFragment fragment, RenderFragment additionalFragment)
      {
         RenderFragment renderFragment = (builder) =>
         {
            builder.AddContent(0, additionalFragment);
            builder.AddContent(1, fragment);
         };
         return renderFragment;
      }

      public static RenderFragment GetSfDataManager<TModel>(string action,List<string> includeFields = null)
      {
         RenderFragment dataManager = (builder) =>
         {
            builder.OpenComponent<Syncfusion.Blazor.Data.SfDataManager>(16);
            builder.AddAttribute(17, "Adaptor", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Syncfusion.Blazor.Adaptors>(Adaptors.CustomAdaptor));
            builder.AddAttribute(18, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) =>
            {
               __builder2.OpenComponent<DataAdaptorComponent<TModel>>(19);
               __builder2.AddAttribute(20, "Action", action);
               if (includeFields!=null)
               {
                  __builder2.AddAttribute(21, "IncludeFields", includeFields);
               }
               __builder2.CloseComponent();
            }
            ));
            builder.CloseComponent();
         };
         return dataManager;

      }

      public static RenderFragment GetSelectorFieldSettings<TFieldSettingsType>(string textField, string valueField) where TFieldSettingsType : SfDataBoundComponent
      {
         RenderFragment settings = (builder) =>
         {
            builder.OpenComponent<TFieldSettingsType>(22);
            builder.AddAttribute(23, "Text", textField);
            builder.AddAttribute(24, "Value", valueField);
            builder.CloseComponent();
         };
         return settings;
      }

      public static RenderFragment CreateSfSwitch<TChecked>(int seq, int __seq0, TChecked __arg0, int __seq1, global::Microsoft.AspNetCore.Components.EventCallback<TChecked> __arg1, int __seq2, global::System.Linq.Expressions.Expression<global::System.Func<TChecked>> __arg2)
      {
         RenderFragment switchFragment = (builder) =>
         {
            builder.OpenComponent<global::Syncfusion.Blazor.Buttons.SfSwitch<TChecked>>(seq);
            builder.AddAttribute(__seq0, "Checked", __arg0);
            builder.AddAttribute(__seq1, "CheckedChanged", __arg1);
            builder.AddAttribute(__seq2, "CheckedExpression", __arg2);
            builder.CloseComponent();
         };
         return switchFragment;
      }

   }
}