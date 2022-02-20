using System.Threading;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Daniel.Blazor.Components
{
   public interface IFormEditor
   {
      IFormEditor FormFather{get;set;}

      Task Refresh();

      bool DisableButtons{get;set;}

   }
}