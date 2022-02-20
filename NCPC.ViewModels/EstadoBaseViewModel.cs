using System.ComponentModel.DataAnnotations;

namespace NCPC.ViewModels
{
    public class EstadoBaseViewModel : NcpcViewModel
    {
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int? Codigo { get; set; }
        public int? CodigoIbge { get; set; }

        [Required]
        public int? IdPais { get; set; }

        //public PaisBaseViewModel Pais { get; set; }
    }
}
