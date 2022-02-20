using Microsoft.AspNetCore.Components;
using Daniel.Blazor.Components;
using Daniel.Utils;

using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Daniel.Blazor.Library
{
   public class FormBuilder<TViewModel> where TViewModel : class, new()
   {

      public delegate Task SelectionChanged<TValue>(TValue args);

      public delegate Task ValueChanged<TValue>(TValue args);

      public List<RenderFragment> Fragments { get; set; }
      public TViewModel Value { get; set; }

      public string DefaultCssCol { get; set; }

      public FormBuilder(TViewModel value)
      {
         Fragments = new List<RenderFragment>();
         Value = value;
         if (DefaultCssCol == null)
            DefaultCssCol = "col-xl-6";
      }

      public FormBuilder(TViewModel value, string defaultCssCol) : this(value)
      {
         DefaultCssCol = defaultCssCol;
      }


      public RenderFragment CreateFormEditorComponent()
      {
         var row = AddToRow(Fragments);
         var fragments = new List<RenderFragment>();
         fragments.Add(row);
         RenderFragment renderFragment = (builder) =>
         {
            builder.OpenComponent<FormEditor<TViewModel>>(0);
            builder.AddAttribute(0, "TValue", Value);
            builder.AddAttribute(1, "FormFragments", fragments);
            builder.CloseComponent();
         };

         return renderFragment;

      }

      public FormBuilder<TViewModel> MakeRowFragment()
      {
         var row = AddToRow(Fragments);
         Fragments = new List<RenderFragment>();
         Fragments.Add(row);
         return this;
      }


      public RenderFragment RenderFragments()
      {
         RenderFragment renderFragment = (builder) =>
         {
            for (int i = 0; i < Fragments.Count; i++)
               builder.AddContent(i + 100, Fragments[i]);
         };

         return renderFragment;
      }

      public FormBuilder<TViewModel> ClearFragments()
      {
         Fragments.Clear();
         return this;
      }


      public RenderFragment GetTimePickerComponent<TValue>(SfTimePicker<TValue> timePicker)
      {
         var props = timePicker.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
         int count = 50;

         RenderFragment dateTimePickerRender = (builder) =>
         {
            builder.OpenComponent<Syncfusion.Blazor.Calendars.SfTimePicker<DateTime?>>(48);

            foreach (PropertyInfo prop in props)
            {
               var objValue = prop.GetValue(timePicker);
               if (objValue != null)
               {
                  builder.AddAttribute(count, prop.Name, objValue);
               }
               count++;
            }

            builder.CloseComponent();
         };
         return dateTimePickerRender;
      }

      public FormBuilder<TViewModel> AddFragmentToCol(RenderFragment fragment, string cssClass = null)
      {
         var column = fragment.InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);
         Fragments.Add(column);
         return this;
      }

      public FormBuilder<TViewModel> AddTimePicker<TValue>(SfTimePicker<TValue> timePicker, Expression<Func<TViewModel, object>> bindValue, string cssClass = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         var existingValue = (TValue)bindValue.Compile().Invoke(Value);
         timePicker.Value = existingValue;
         var callback = EventCallback.Factory.Create<TValue>(this, args =>
            {
               prop.SetValue(Value, args);
               timePicker.OnChange.InvokeAsync(new ChangeEventArgs() { Value = Value });
            });
         timePicker.ValueChanged = callback;
         //TODO localizar
         timePicker.Placeholder = $"Informe um" + prop.GetDisplayName();
         RenderFragment dateTimePickerFragment = GetTimePickerComponent<TValue>(timePicker);

         RenderFragment labelFragment = (builder) => builder.AddLabel(prop, 0);
         var column = labelFragment.Append(dateTimePickerFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);
         Fragments.Add(column);
         return this;

      }

      public RenderFragment GetDatePickerComponent<TValue>(SfDatePicker<TValue> dateTimePicker)
      {
         var props = dateTimePicker.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
         int count = 50;

         RenderFragment dateTimePickerRender = (builder) =>
         {
            builder.OpenComponent<Syncfusion.Blazor.Calendars.SfDatePicker<DateTime?>>(48);

            foreach (PropertyInfo prop in props)
            {
               var objValue = prop.GetValue(dateTimePicker);
               if (objValue != null)
               {
                  builder.AddAttribute(count, prop.Name, objValue);
               }
               count++;
            }

            builder.CloseComponent();
         };
         return dateTimePickerRender;
      }

      public FormBuilder<TViewModel> AddDatePicker<TValue>(SfDatePicker<TValue> datePicker, Expression<Func<TViewModel, object>> bindValue, string cssClass = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         var existingValue = (TValue)bindValue.Compile().Invoke(Value);
         datePicker.Value = existingValue;
         var callback = EventCallback.Factory.Create<TValue>(this, args =>
            {
               prop.SetValue(Value, args);
               datePicker.OnChange.InvokeAsync(new ChangeEventArgs() { Value = Value });
            });
         datePicker.ValueChanged = callback;
         //TODO localizar
         datePicker.Placeholder = $"Informe um " + prop.GetDisplayName();
         RenderFragment dateTimePickerFragment = GetDatePickerComponent<TValue>(datePicker);
         RenderFragment labelFragment = (builder) => builder.AddLabel(prop, 0);
         var column = labelFragment.Append(dateTimePickerFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);
         Fragments.Add(column);
         return this;

      }



      private RenderFragment GetDateTimePickerComponent<TValue>(SfDateTimePicker<TValue> dateTimePicker)
      {
         var props = dateTimePicker.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
         int count = 50;

         RenderFragment dateTimePickerRender = (builder) =>
         {
            builder.OpenComponent<Syncfusion.Blazor.Calendars.SfDateTimePicker<TValue>>(48);

            foreach (PropertyInfo prop in props)
            {
               var objValue = prop.GetValue(dateTimePicker);
               if (objValue != null)
               {
                  builder.AddAttribute(count, prop.Name, objValue);
               }
               count++;
            }

               // builder.AddAttribute(49, "Placeholder", "Choose a Date");
               // builder.AddAttribute(50, "Min", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.DateTime>(DateTime.Now.AddDays(-1)));
               // builder.AddAttribute(51, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<DateTime?>(Agendamento.HorarioInicial));
               // builder.AddAttribute(52, "ValueChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<DateTime?>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<DateTime?>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => Agendamento.HorarioInicial = __value, Agendamento.HorarioInicial))));
               // builder.AddAttribute(53, "ValueExpression", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Linq.Expressions.Expression<System.Func<DateTime?>>>(() => Agendamento.HorarioInicial));
               builder.CloseComponent();
         };
         return dateTimePickerRender;
      }

      public RenderFragment GetSfNumericTextBoxComponent<TValue>(SfNumericTextBox<TValue> numericTextBox)
      {
         var props = numericTextBox.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
         int count = 50;

         RenderFragment numericRender = (builder) =>
         {
            builder.OpenComponent<SfNumericTextBox<TValue>>(48);

            foreach (PropertyInfo prop in props)
            {
               var objValue = prop.GetValue(numericTextBox);
               if (objValue != null)
               {
                  builder.AddAttribute(count, prop.Name, objValue);
               }
               count++;
            }

            builder.CloseComponent();
         };
         return numericRender;
      }

      public FormBuilder<TViewModel> AddNumericTextBox<TValue>(SfNumericTextBox<TValue> numericTextBox, Expression<Func<TViewModel, object>> bindValue, string headerText = null, string cssClass = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         var existingValue = (TValue)bindValue.Compile().Invoke(Value);
         numericTextBox.Value = existingValue;
         var callback = EventCallback.Factory.Create<TValue>(this, args =>
         {
            prop.SetValue(Value, args);
            numericTextBox.OnChange.InvokeAsync(new ChangeEventArgs() { Value = Value });
         });
         numericTextBox.ValueChanged = callback;
         //TODO localizar
         numericTextBox.Placeholder = $"Informe um " + prop.GetDisplayName();
         RenderFragment dateTimePickerFragment = GetSfNumericTextBoxComponent<TValue>(numericTextBox);
         RenderFragment labelFragment = (builder) =>
         {
            if (headerText!=null)
               builder.AddLabel(headerText, 0);
            else
               builder.AddLabel(prop, 0);
         };
         var column = labelFragment.Append(dateTimePickerFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);
         Fragments.Add(column);
         return this;

      }


      public FormBuilder<TViewModel> AddDateTimePicker<TValue>(SfDateTimePicker<TValue> dateTimePicker, Expression<Func<TViewModel, object>> bindValue, string cssClass = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         var existingValue = (TValue)bindValue.Compile().Invoke(Value);
         dateTimePicker.Value = existingValue;
         var callback = EventCallback.Factory.Create<TValue>(this, args =>
            {
               prop.SetValue(Value, args);
            });
         dateTimePicker.ValueChanged = callback;
         //TODO localizar
         dateTimePicker.Placeholder = $"Informe um " + prop.GetDisplayName();
         RenderFragment dateTimePickerFragment = GetDateTimePickerComponent<TValue>(dateTimePicker);
         RenderFragment labelFragment = (builder) => builder.AddLabel(prop, 0);
         var column = labelFragment.Append(dateTimePickerFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);
         Fragments.Add(column);
         return this;

      }


      public FormBuilder<TViewModel> AddTextBox(Expression<Func<TViewModel, object>> func, string cssClass = null, InputType inputType = InputType.Text
      , bool readOnly = false, Expression<Func<string>> validationFunc = null, string placeHolder = null, ValueChanged<string> valueChanged = null
      , Dictionary<string, object> htmlAttributes = null, string headerText = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(func);

         RenderFragment labelFragment = (builder) => {
           if (headerText!=null)
           builder.AddLabel(headerText, 0);
           else
            builder.AddLabel(prop, 0);
         };

         //var teste = func.GetParentValue<TViewModel>(Value);
         RenderFragment renderFragment = (builder) =>
         {
            Console.WriteLine("Value:" + Value != null);

            string existingValue = func.Compile().Invoke(Value)?.ToString() + "";
            Console.WriteLine("Existing value:" + existingValue);
            builder.OpenComponent<SfTextBox>(1);
            builder.AddAttribute(0, "Value", existingValue);

            var callback = EventCallback.Factory.Create<ChangedEventArgs>(this, args =>
            {
             prop.SetValue(Value, args.Value);
             valueChanged?.Invoke(args.Value);
          });

            builder.AddAttribute(1, "ValueChange", callback);
            builder.AddAttribute(2, "Type", inputType);
            builder.AddAttribute(2, "ReadOnly", readOnly);
            builder.AddAttribute(4, "Placeholder", placeHolder);
            builder.AddAttribute(5, "HtmlAttributes", htmlAttributes);
            builder.CloseComponent();

            if (!readOnly)//Somente para leitura não deve ser validado no formulário, uma vez que é impossivel o usuário corrigir
               {
               if (validationFunc != null)
                  builder.AddValidationMessageFor<string>(validationFunc);
               else
                  builder.AddValidationMessageFor<string>(prop, Value);
            }

         };

         var column = labelFragment.Append(renderFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);

         Fragments.Add(column);
         return this;
      }

      public FormBuilder<TViewModel> AddSwitch(Expression<Func<TViewModel, object>> func, string cssClass = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(func);
         if (prop.PropertyType == typeof(bool?))
            return AddSwitch<bool?>(func, cssClass);
         else
            return AddSwitch<bool>(func, cssClass);
      }

      public FormBuilder<TViewModel> AddSwitch<TType>(Expression<Func<TViewModel, object>> func, string cssClass = null, ValueChanged<TType> valueChanged = null)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(func);

         RenderFragment labelFragment = (builder) => builder.AddLabel(prop, 0);

         var existingValue = prop.GetValue(Value);
         var callback = EventCallback.Factory.Create<TType>(this, args =>
            {
               prop.SetValue(Value, args);
               valueChanged?.Invoke(args);
            });

         var parameter = Expression.Constant(Value);
         var body = Expression.MakeMemberAccess(parameter, prop);
         var resultToReturn = Expression.Lambda(body);

         var switchFragment = BlazorExtension.CreateSfSwitch<TType>(16, 17, (TType)existingValue, 18, callback, 19, (Expression<Func<TType>>)resultToReturn);
         switchFragment.Append((builder) => { builder.AddValidationMessageFor<TType>(prop, Value); });

         var column = labelFragment.Append(switchFragment.InsertIntoDiv("switch-forms-container"))
                                   .InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);

         Fragments.Add(column);
         return this;

      }

      /// <summary>
      /// Adiciona um componente do Tipo DropDown
      /// </summary>
      /// <param name="bindValue">Propriedade que guardará o valor</param>
      /// <param name="textField">Propriedade do TItem que irá exibir a informação no DropDown</param>
      /// <param name="valueField">Propriedade do TItem de onde o valor será atribuído</param>
      /// <param name="dataManagerAction">Uma string com o caminho a ser buscado na API</param>
      /// <param name="allowFiltering">Permite filtro no dropDown</param>
      /// <param name="cssClass">Classe css com a Colouna (Ex: col-lg-4)</param>
      /// <param name="query">Objeto Query para filtrar por padrão</param>
      /// <typeparam name="TItem"></typeparam>
      /// <returns></returns>
      public FormBuilder<TViewModel> AddDropDown<TItem>(Expression<Func<TViewModel, object>> bindValue,
      Expression<Func<TItem, object>> textField, Expression<Func<TItem, object>> valueField, string dataManagerAction,
      bool allowFiltering = false, string cssClass = null, Query query = null, SelectionChanged<TItem> onChanged = null, bool enabled = true, bool showClearButton = false,
      Action<object> ComponentReference = null, string placeholder = null, string headerText = null, List<string> includeFields =null) where TItem : class
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         if (prop.PropertyType == typeof(int?))
            return AddSelector<int?, TItem>(false, bindValue, textField, valueField, dataManagerAction, allowFiltering, cssClass, query, onChanged, enabled, showClearButton, ComponentReference, placeholder, headerText,includeFields);
         else
            return AddSelector<int, TItem>(false, bindValue, textField, valueField, dataManagerAction, allowFiltering, cssClass, query, onChanged, enabled, showClearButton, ComponentReference, placeholder, headerText,includeFields);

      }

      public FormBuilder<TViewModel> AddComboBox<TItem>(Expression<Func<TViewModel, object>> bindValue,
         Expression<Func<TItem, object>> textField, Expression<Func<TItem, object>> valueField, string dataManagerAction, bool allowFiltering = true,
         string cssClass = null, Query query = null, SelectionChanged<TItem> onChanged = null, bool enabled = true, bool showClearButton = false
          , string placeholder = null, string headerText = null, Action<object> ComponentReference = null,
          List<string> includeFields =null) where TItem : class
      {
         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         if (prop.PropertyType == typeof(int?))
            return AddSelector<int?, TItem>(true, bindValue, textField, valueField, dataManagerAction, allowFiltering, cssClass, query, onChanged, enabled, showClearButton, ComponentReference, placeholder, headerText,includeFields);
         else
            return AddSelector<int, TItem>(true, bindValue, textField, valueField, dataManagerAction, allowFiltering, cssClass, query, onChanged, enabled, showClearButton, ComponentReference, placeholder, headerText,includeFields);

      }


      private FormBuilder<TViewModel> AddSelector<TValueType, TItem>(bool comboBox, Expression<Func<TViewModel, object>> bindValue,
         Expression<Func<TItem, object>> textField, Expression<Func<TItem, object>> valueField, string dataManagerAction,
         bool allowFiltering = false, string cssClass = null, Query query = null, SelectionChanged<TItem> onChanged = null, bool enabled = true, bool showClearButton = false
         , Action<object> ComponentReference = null, string placeholder = null, string headerText = null, List<string> includeFields =null) where TItem : class
      {

         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);

         RenderFragment labelFragment = (builder) =>
         {
            if (headerText != null)
               builder.AddLabel(headerText, 0);
            else
               builder.AddLabel(prop, 0);
         };
         var dropDownFragment = GetSelector<TValueType, TItem>(comboBox, bindValue, textField, valueField, dataManagerAction, allowFiltering,
               cssClass, query, onChanged, enabled, showClearButton, ComponentReference, placeholder, headerText,includeFields);
         dropDownFragment = dropDownFragment.Append((builder) => builder.AddValidationMessageFor<TValueType>(prop, Value));


         var column = labelFragment.Append(dropDownFragment).InsertIntoColumn(cssClass != null ? cssClass : DefaultCssCol);

         Fragments.Add(column);
         return this;
      }

      public RenderFragment GetSelector<TValueType, TItem>(bool comboBox, Expression<Func<TViewModel, object>> bindValue,
        Expression<Func<TItem, object>> textField, Expression<Func<TItem, object>> valueField, string dataManagerAction,
        bool allowFiltering = false, string cssClass = null, Query query = null, SelectionChanged<TItem> onChanged = null, bool enabled = true, bool showClearButton = false
        , Action<object> ComponentReference = null, string placeholder = null, string headerText = null, List<string> includeFields =null) where TItem : class
      {

         var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);
         var textProp = ExpressionUtil.GetPropertyInfo<TItem, object>(textField);
         var valueProp = ExpressionUtil.GetPropertyInfo<TItem, object>(valueField);

         //Console.WriteLine("Adicionando Label:" + prop.GetDisplayName());


         RenderFragment dropDownFragment = (builder) =>
         {

            var existingValue = (TValueType)prop.GetValue(Value);
               //Console.WriteLine($"Existing Value for {prop.Name}: {existingValue}");
               //Console.WriteLine("typeof:"+existingValue.GetType().ToString());
               //TODO Localizar o PlaceHolder
               if (comboBox)
            {
               builder.OpenComponent<SfComboBox<TValueType, TItem>>(13);
            }
            else
            {
               builder.OpenComponent<SfDropDownList<TValueType, TItem>>(13);
            }



            if (query != null)
               builder.AddAttribute(0, "Query", query);
            var parameter = Expression.Constant(Value);
            var body = Expression.MakeMemberAccess(parameter, prop);
            var resultToReturn = Expression.Lambda(body);
            builder.AddAttribute(0, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<TValueType>(existingValue));

            builder.AddAttribute(1, "ValueExpression", resultToReturn);

            var callback = EventCallback.Factory.Create<TValueType>(this, args =>
              {
               prop.SetValue(Value, args);
               if (args == null)
                  onChanged?.Invoke((TItem)null);

            });

            builder.AddAttribute(2, "ValueChanged", callback);
            builder.AddAttribute(14, "Placeholder", $"Selecione um " + (placeholder != null ? placeholder : prop.GetDisplayName()));
            builder.AddAttribute(15, "ShowClearButton", showClearButton);

            if (allowFiltering)
               builder.AddAttribute(16, "AllowFiltering", true);

            var fieldSettings = comboBox ?
               BlazorExtension.GetSelectorFieldSettings<ComboBoxFieldSettings>(textProp.Name, valueProp.Name) :
               BlazorExtension.GetSelectorFieldSettings<DropDownListFieldSettings>(textProp.Name, valueProp.Name);

            builder.AddAttribute(17, "ChildContent", BlazorExtension.GetSfDataManager<TItem>(dataManagerAction,includeFields).Append(fieldSettings).Append(builder3 =>
            {
             builder3.OpenComponent<Syncfusion.Blazor.DropDowns.DropDownListEvents<TValueType, TItem>>(14);
             var onChangeCallback = EventCallback.Factory.Create<ChangeEventArgs<TValueType, TItem>>(this, args =>
             {
              onChanged?.Invoke((TItem)args.ItemData);
           });

             builder3.AddAttribute(15, "ValueChange", onChangeCallback);
             builder3.CloseComponent();

          }));
            builder.AddAttribute(17, "Enabled", enabled);

            builder.AddComponentReferenceCapture(1, a => ComponentReference?.Invoke(a));

            builder.CloseComponent();
         };
         return dropDownFragment;

      }


      private RenderFragment AddToRow(List<RenderFragment> childFragments)
      {
         RenderFragment renderFragment = (builder) =>
        {
           builder.OpenElement(0, "div");
           builder.AddAttribute(0, "class", "row");
           for (int i = 0; i < childFragments.Count; i++)
              builder.AddContent(i + 1, childFragments[i]);
           builder.CloseComponent();

        };
         return renderFragment;
      }

      public void SetValue(object value)
      {
      }


   }
}

//Exemplo de construtor de formulários
//Achei este exemplo depois de criar este formbuilder, tem uma ideia parecida porem com algumas diferenças
//https://www.syncfusion.com/blogs/post/how-to-create-a-dynamic-form-builder-in-blazor.aspx