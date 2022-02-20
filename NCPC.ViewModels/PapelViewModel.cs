using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCPC.ViewModels
{
    public class PapelViewModel : NcpcViewModel
    {
        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

    }
}
