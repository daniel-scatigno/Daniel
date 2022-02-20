using System.ComponentModel.DataAnnotations;
using System;

namespace Daniel.ViewModels
{
    public class NotificacaoBaseViewModel : DanielViewModel
    {        

       public int IdLancamento{get;set;}
       
       public string IconeCss{get;set;}

       public string Titulo{get;set;}

       public string Descricao{get;set;}

       public DateTime DataCriacao{get;set;}

       public bool Visualizado{get;set;}

      //Acessador
       public bool Lido {get;set;}

       public string Link {get;set;}
    }
}
