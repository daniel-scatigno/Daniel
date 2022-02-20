using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using NCPC.Utils;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace NCPC.Blazor.Library
{

    public class GridBuilder<TViewModel> where TViewModel : class
    {

        public delegate Task RowDoubleClick(RecordDoubleClickEventArgs<TViewModel> args);
        public delegate Task RowSelected(RowSelectEventArgs<TViewModel> args);

        public List<RenderFragment> Fragments { get; set; }
        public List<GridColumn> Columns { get; set; }

        public GridEvents<TViewModel> GridEvents { get; set; }

        public string DefaultCssCol { get; set; }

        public GridBuilder()
        {
            Fragments = new List<RenderFragment>();
            GridEvents = new GridEvents<TViewModel>();

        }

        public GridBuilder<TViewModel> MakeGrid()
        {
            var row = AddToRow(Fragments);
            Fragments = new List<RenderFragment>();
            Fragments.Add(row);
            return this;
        }

        public GridBuilder<TViewModel> CreateGrid(string Action, bool allowSelection = true, bool allowSorting = true, bool allowFiltering = true,
        bool enableHover = true, bool allowGrouping = true, bool enableAltRow = true, bool allowPaging = true, Double rowHeight = 38, int pageSize = 10)
        {
            RenderFragment gridFragment = (builder) =>
            {

                builder.OpenComponent<Syncfusion.Blazor.Grids.SfGrid<TViewModel>>(1);
                builder.AddAttribute(41, "AllowSelection", RuntimeHelpers.TypeCheck<System.Boolean>(allowSelection));
                builder.AddAttribute(42, "AllowSorting", RuntimeHelpers.TypeCheck<System.Boolean>(true));
                builder.AddAttribute(43, "AllowFiltering", RuntimeHelpers.TypeCheck<System.Boolean>(true));
                builder.AddAttribute(44, "EnableHover", RuntimeHelpers.TypeCheck<System.Boolean>(true));
                builder.AddAttribute(45, "RowHeight", RuntimeHelpers.TypeCheck<System.Double>(38));
                builder.AddAttribute(46, "AllowGrouping", RuntimeHelpers.TypeCheck<System.Boolean>(true));
                builder.AddAttribute(47, "EnableAltRow", RuntimeHelpers.TypeCheck<System.Boolean>(true));
                builder.AddAttribute(48, "AllowPaging", RuntimeHelpers.TypeCheck<System.Boolean>(true));

                builder.AddAttribute(49, "ChildContent", (RenderFragment)((builder2) =>
            {
                builder2.AddContent(2, BlazorExtension.GetSfDataManager<TViewModel>(Action));
                builder2.OpenComponent<Syncfusion.Blazor.Grids.GridFilterSettings>(56);
                builder2.AddAttribute(57, "Type", RuntimeHelpers.TypeCheck<Syncfusion.Blazor.Grids.FilterType>(Syncfusion.Blazor.Grids.FilterType.Menu));
                builder2.CloseComponent();
                builder2.OpenComponent<Syncfusion.Blazor.Grids.GridPageSettings>(59);
                builder2.AddAttribute(60, "PageSize", RuntimeHelpers.TypeCheck<System.Int32>(pageSize));
                builder2.CloseComponent();
                if (GridEvents != null)
                {
                    builder2.OpenComponent<Syncfusion.Blazor.Grids.GridEvents<TViewModel>>(62);
                    if (GridEvents.RowSelected.HasDelegate)
                    {
                        builder2.AddAttribute(63, "RowSelected", GridEvents.RowSelected);
                    }

                    if (GridEvents.OnRecordDoubleClick.HasDelegate)
                    {
                        builder2.AddAttribute(64, "OnRecordDoubleClick", GridEvents.OnRecordDoubleClick);
                    }
                    builder2.CloseComponent();
                }
                builder2.OpenComponent<Syncfusion.Blazor.Grids.GridColumns>(66);
                builder2.AddAttribute(67, "ChildContent", (RenderFragment)((builder3) =>
             {
                 foreach (GridColumn column in Columns)
                     AddColumnToGrid(builder3, column);

             }));

                builder2.CloseComponent();


            }));
                builder.CloseComponent();


            };
            Fragments.Add(gridFragment);
            return this;
        }

        public RenderFragment RenderColumns()
        {
            RenderFragment columns = (builder) =>
            {
                foreach (GridColumn column in Columns)
                    AddColumnToGrid(builder, column);
            };
            return columns;

        }

        private void AddColumnToGrid(RenderTreeBuilder builder, GridColumn column)
        {
            builder.OpenComponent<Syncfusion.Blazor.Grids.GridColumn>(68);
            int count = 70;
            var props = column.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ParameterAttribute), true).Any());
            foreach (PropertyInfo prop in props)
            {
                var objValue = prop.GetValue(column);
                if (objValue != null)
                {
                    builder.AddAttribute(count, prop.Name, objValue);
                }
                count++;
            }

            builder.CloseComponent();

        }

        public GridBuilder<TViewModel> AddColumn(GridColumn column)
        {
            if (Columns == null)
                Columns = new List<GridColumn>();
            Columns.Add(column);
            return this;
        }

        public GridBuilder<TViewModel> AddColumn(Expression<Func<TViewModel, object>> bindValue, string width = null,
         string headerText = null, FilterSettings filterSettings = null, string format = null, bool allowSorting = true,
          TextAlign textAlign = TextAlign.Left,IDictionary<string,object> customAttributes = null)
        {
            var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);

            AddColumn(new GridColumn()
            {
                Field = bindValue.ToFieldName(),
                HeaderText = headerText == null ? prop.GetDisplayName() : headerText,
                Width = width,
                FilterSettings = filterSettings,
                Format = format,
                AllowSorting = allowSorting,
                TextAlign = textAlign,
                CustomAttributes = customAttributes,
                HeaderTextAlign = textAlign
            });
            return this;
        }

        public GridBuilder<TViewModel> AddColumn(string field, string headerText, Expression<Func<TViewModel,
         object>> displayValue, string width = null, FilterSettings filterSettings = null,
          bool allowSorting = true, TextAlign textAlign = TextAlign.Left,IDictionary<string,object> customAttributes = null)
        {

            var column = new GridColumn()
            {
                Field = field,
                HeaderText = headerText,
                Width = width,
                FilterSettings = filterSettings,
                AllowSorting = allowSorting,
                TextAlign = textAlign,
                CustomAttributes = customAttributes,
                HeaderTextAlign = textAlign
            };

            column.Template = (RenderFragment<object>)((context) => (builder3) =>
            {
                var value = (context as TViewModel);
                builder3.AddContent(0, displayValue.Compile().Invoke(value).ToString());

            });
            AddColumn(column);

            return this;

        }

        public GridBuilder<TViewModel> AddColumn(Expression<Func<TViewModel, object>> bindValue, Expression<Func<TViewModel,
         object>> displayValue, string width = null, string headerText = null,FilterSettings filterSettings = null,
          bool allowSorting = true, TextAlign textAlign = TextAlign.Left,IDictionary<string,object> customAttributes = null)
        {
            var prop = ExpressionUtil.GetPropertyInfo<TViewModel, object>(bindValue);

            AddColumn(bindValue.ToFieldName(), headerText == null ? prop.GetDisplayName() : headerText,
             displayValue, width, filterSettings, allowSorting, textAlign,customAttributes);

            return this;
        }

        private GridBuilder<TViewModel> AddGridEvents(GridEvents<TViewModel> gridEvents)
        {
            GridEvents = gridEvents;
            return this;
        }

        public GridBuilder<TViewModel> AddEventRowSelected(RowSelected rowSelected)
        {
            if (GridEvents == null)
                GridEvents = new GridEvents<TViewModel>();

            GridEvents.RowSelected = EventCallback.Factory.Create<RowSelectEventArgs<TViewModel>>(this, args => { rowSelected(args); });
            return this;

        }

        public GridBuilder<TViewModel> AddEventRowDoubleClick(RowDoubleClick rowDoubleClick)
        {
            if (GridEvents == null)
                GridEvents = new GridEvents<TViewModel>();

            GridEvents.OnRecordDoubleClick = EventCallback.Factory.Create<RecordDoubleClickEventArgs<TViewModel>>(this, args => { rowDoubleClick(args); }); ;
            return this;

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
//Achei este exemplo depois de criar este GridBuilder, tem uma ideia parecida porem com algumas diferenças
//https://www.syncfusion.com/blogs/post/how-to-create-a-dynamic-form-builder-in-blazor.aspx