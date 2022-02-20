using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using NCPC.Infra;
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
   public abstract class BaseApiController : Controller
   {

      [NonAction]
      public override OkObjectResult Ok([ActionResultObjectValue] object value)
      {
         return new OkObjectResult(new { StatusCode = StatusCodes.Status200OK, Result = value });
      }

      [NonAction]
      public new OkObjectResult Ok()
      {
         return new OkObjectResult(new { StatusCode = StatusCodes.Status200OK });
      }

      [NonAction]
      public override ConflictObjectResult Conflict(object error)
      {
         return new ConflictObjectResult(new { StatusCode = StatusCodes.Status409Conflict, Result = error });
      }

      [NonAction]
      public override BadRequestObjectResult BadRequest(object error)
      {
         return new BadRequestObjectResult(new { StatusCode = StatusCodes.Status400BadRequest, Result = error });
      }

      [NonAction]
      public new NotFoundObjectResult NotFound()
      {
         return new NotFoundObjectResult(new { StatusCode = StatusCodes.Status404NotFound });
      }
   }
}