namespace Daniel.Domain.Models
{
    public abstract class PaisBase : BaseModel
    {
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public int? Codigo { get; set; }
        public int? CodigoSiscomex { get; set; }
        public string SiglaLinguagem { get; set; }
        public string SiglaMoeda { get; set; }
        public string IdentificadorRegistro { get; set; }
        //public ICollection<EstadoBase> EstadosBase { get; set; }
    }
}
