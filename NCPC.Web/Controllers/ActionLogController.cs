using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NCPC.Web.Controllers;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NCPC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Syncfusion.Blazor;
using NCPC.Domain.Models;
using NCPC.ViewModels;
using NCPC.Resource.Resources;
using Syncfusion.Blazor.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using NCPC.Infra;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace SafePosition.API.Controllers
{
   [ApiController]
   [Authorize]
   public class ActionLogController : ApiController<ActionLog, ActionLogViewModel>
   {
      
      public IConfiguration Config { get; set; }

      public ActionLogController(IUnitOfWork uow, IMapper mapper, IConfiguration config) : base(uow, uow.GetRepository<ActionLogRepository>(), mapper)
      {
         Config = config;
      }
   }
}