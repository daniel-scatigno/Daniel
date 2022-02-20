using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCPC.Resource.Resources;

namespace NCPC.ViewModels
{

    public class UploadedFileViewModel
    {        
       public string FileName{get;set;}
       public byte[] Content{get;set;}
       public string FileType{get;set;}
    }
}
