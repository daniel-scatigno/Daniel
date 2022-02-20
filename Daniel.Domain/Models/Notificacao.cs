using Daniel.Domain.Models;
using System;
using System.Collections.Generic;

namespace Daniel.Domain.Models
{
    public class NotificacaoBase : BaseModel
    {
       public int IdLancamento{get;set;}

       public int IdUsuario{get;set;}
       
       public string IconeCss{get;set;}

       public string Titulo{get;set;}

       public string Descricao{get;set;}

       public DateTime DataCriacao{get;set;}

       public bool Visualizado{get;set;}

       public bool Lido {get;set;}

       public string Link {get;set;}

       

    }
}