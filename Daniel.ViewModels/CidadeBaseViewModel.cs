using System.ComponentModel.DataAnnotations;

namespace Daniel.ViewModels
{
    public abstract class CidadeBaseViewModel : DanielViewModel
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
