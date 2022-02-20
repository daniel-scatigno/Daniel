using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daniel.ViewModels
{
   public class UsuarioBaseViewModel:DanielViewModel,IUsuarioViewModel
   {
      public UsuarioBaseViewModel()
      {
         Papeis = new List<PapelViewModel>();
      }
      public string NomeUsuario { get; set; }

      public string Email { get; set; }

      public string Senha { get; set; }

      public DateTime? DataCriacao { get; set; }

      public bool Ativo { get; set; }

      public string Token { get; set; }

      /// <summary>
      /// Papeis do usuário separado por ;
      /// </summary>
      /// <value></value>
      /// 
      [JsonInclude]
      public ICollection<PapelViewModel> Papeis { get; set; }
      public string PapeisDoUsuario{ get; set; }

      public string PermissoesDoUsuario{get;set;}

   }
}