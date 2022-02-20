using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.ViewModels
{

    public class UploadedFileViewModel
    {        
       public string FileName{get;set;}
       public byte[] Content{get;set;}
       public string FileType{get;set;}
    }
}
