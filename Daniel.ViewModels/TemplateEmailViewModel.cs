using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.ViewModels
{
    public class TemplateEmailViewModel:DanielViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Descricao")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Assunto")]
        public string Assunto { get; set; }

        [Required]
        [Display(Name = "Corpo")]
        public string Corpo { get; set; }
    }
}
