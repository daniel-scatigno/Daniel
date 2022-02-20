﻿using NCPC.Domain.Lib.Models;
using System;

namespace NCPC.Domain.Models
{
    public abstract class  CidadeBase : BaseModel
    {
        public string Nome { get; set; }
        public int? CodigoIbge { get; set; }

        public int IdEstado { get; set; }
        //public virtual EstadoBase EstadoBase { get; set; }
    }
}