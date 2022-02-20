using NCPC.Domain.Models;
using System;
using System.Collections.Generic;

namespace NCPC.Domain.Models
{
    public abstract class EstadoBase : BaseModel
    {
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public int? Codigo { get; set; }
        public int? CodigoIbge { get; set; }

        public int IdPais { get; set; }
        //public PaisBase PaisBase { get; set; }

        //public ICollection<CidadeBase> CidadesBase { get; set; }

    }
}