using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCPC.Resource.Resources;

namespace NCPC.ViewModels
{
    public class TemplateEmailViewModel:NcpcViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Descricao", ResourceType = typeof(SfResources))]
        public string Descricao { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Assunto", ResourceType = typeof(SfResources))]
        public string Assunto { get; set; }

        [Required]
        [Display(Name = "Corpo", ResourceType = typeof(SfResources))]
        public string Corpo { get; set; }
    }
}
