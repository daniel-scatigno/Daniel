using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.ViewModels
{
    public class PapelViewModel : DanielViewModel
    {
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

    }
}
