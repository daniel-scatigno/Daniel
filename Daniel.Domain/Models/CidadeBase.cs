using Daniel.Domain.Lib.Models;
using System;

namespace Daniel.Domain.Models
{
    public abstract class  CidadeBase : BaseModel
    {
        public string Nome { get; set; }
        public int? CodigoIbge { get; set; }

        public int IdEstado { get; set; }
        //public virtual EstadoBase EstadoBase { get; set; }
    }
}