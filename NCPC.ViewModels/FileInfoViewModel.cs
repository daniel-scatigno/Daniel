using System.ComponentModel.DataAnnotations;
using System;

namespace NCPC.ViewModels
{
   public class FileInfoViewModel
   {
      public string FileName { get; set; }
      public string Path { get; set; }
      public DateTime Created { get; set; }

      public string Description
      {
         get
         {
            return Created.ToShortDateString() + " - " + FileName;

         }
      }

   }
}
