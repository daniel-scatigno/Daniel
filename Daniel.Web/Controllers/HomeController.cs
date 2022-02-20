using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Daniel.Infra;
using Daniel.Utils;
using AutoMapper;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Cors;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Daniel.ViewModels;
using System.Text.Json;

namespace Daniel.Web.Controllers
{
   public class HomeController : BaseApiController
   {
      private IConfiguration Configuration { get; set; }

      public HomeController(IConfiguration configuraion)
      {
         this.Configuration = configuraion;
      }

      [HttpPost]
      [Route("[Controller]/Encrypt")]
      public string Encrypt(string text)
      {
         return EncryptUtils.Encrypt(text);
      }

      [HttpPost]
      [Route("[Controller]/Decrypt")]      
      public string Decrypt(string text)
      {
         return EncryptUtils.Decrypt(text);
      }

      [NonAction]
      private DirectoryInfo GetLogDirectory()
      {
            var section = Configuration.GetSection("Logging");
         string path = section.GetValue<string>("PathFormat");
         
         string dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
         var paths = path.Split("/").ToList();
         paths.Take(paths.Count()-1).ToList().ForEach(p=>{
            dir = Path.Combine(dir,p);
         });
         
         var dirInfo = new DirectoryInfo(dir);
         if (!dirInfo.Exists)
            dirInfo = new DirectoryInfo(dir.Replace("\\bin\\Debug\\net5.0","").Replace("/bin/Debug/net5.0",""));
         
         return dirInfo;
      }

      [HttpGet]
      [Route("[Controller]/GetLogFiles")]
      [Authorize]
      public ActionResult GetLogFiles()
      {
         var files = GetLogDirectory().EnumerateFiles().ToList().Select(f=>new FileInfoViewModel(){
            FileName = f.Name,
            Path = f.Directory.FullName,
            Created = f.CreationTime
         });
         
         return Ok(files);
      }

      [HttpGet]
      [Route("[Controller]/GetLogContent")]
      [Authorize]
      public ActionResult GetLogContent(string file)
      {
         var fileInfo = GetLogDirectory().EnumerateFiles().Where(x=>x.Name==file).FirstOrDefault();
         var stream = System.IO.File.Open(fileInfo.FullName,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
         var reader = new StreamReader(stream);
         string content = reader.ReadToEnd();
         
         //string content = System.IO.File.ReadAllText(fileInfo.FullName);
         
         return Ok(content);
      }

      [HttpGet]
      [HttpPost]
      [Route("[controller]/Teste")]
      public IActionResult Teste([FromQuery]string tabela,[FromBody] Newtonsoft.Json.Linq.JObject json)
      {
         return Ok("ok");

      }

   }
}