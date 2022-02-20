using System.Collections;
using System.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Data.Common;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using NCPC.Blazor.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using NCPC.Utils;
namespace NCPC.Blazor.Components
{
    public partial class DataAdaptorComponent<TModel> : DataAdaptor<TModel>
    {
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        protected JsonSerializerOptions JsonOptions { get; set; }
               

        [Inject]
        public DataService DataService { get; set; }

        [Parameter]
        public string Action { get; set; }

        [Parameter]
        public EventCallback<List<TModel>> DataRead { get; set; }
        
        [Parameter]
        public EventCallback<DataManagerRequest> BeforeDataRead { get; set; }

        public DataResult DataResult{get;set;}

        [Parameter]
        public List<string> IncludeFields{get;set;}


        public DataAdaptorComponent() : base()
        {
            JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            JsonOptions.ReferenceHandler = ReferenceHandler.Preserve;
        }
        protected override async Task OnInitializedAsync()
        {
            //Important, necessário para carregar os dados
            await base.OnInitializedAsync();
        }

        public override async Task<object> ReadAsync(DataManagerRequest dm, string key = null)
        {
            //Console.WriteLine("Action ReadAsync:" + Action);

            await BeforeDataRead.InvokeAsync(dm);
            
            if(IncludeFields!=null && IncludeFields.Any())
               dm.Select=IncludeFields;

            DataResult = await DataService.ToDataResult($"{Action}", dm);

            DataResult.Result = DataService.ToListResult<TModel>(DataResult);

            if (dm.Group != null)
            {
                IEnumerable GroupData = Enumerable.Empty<object>();
                foreach (var group in dm.Group)
                {
                    DataResult.Result = DataUtil.Group<TModel>(DataResult.Result, group, dm.Aggregates, 0, dm.GroupByFormatter);
                }

                return dm.RequiresCounts ? DataResult : (object)DataResult.Result;
            }

            await DataRead.InvokeAsync((List<TModel>)DataResult.Result);            
            return dm.RequiresCounts ? DataResult : (List<TModel>)DataResult.Result;
        }



    }
}