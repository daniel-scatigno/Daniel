using System.ComponentModel.DataAnnotations;

namespace NCPC.ViewModels
{
    public abstract class CidadeBaseViewModel : NcpcViewModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int? CodigoIbge { get; set; }

        [Required]
        public int? IdEstado { get; set; }

        // public EstadoBaseViewModel Estado { get; set; }
    }
}
