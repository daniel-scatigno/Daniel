using System;
using System.Collections;
using System.Collections.Generic;
namespace NCPC.ViewModels
{
   public interface IUsuarioViewModel
   {
      int Id { get; set; }
      string NomeUsuario { get; set; }

      string Email { get; set; }

      string Senha { get; set; }

      DateTime? DataCriacao { get; set; }

      bool Ativo { get; set; }

      string Token { get; set; }

      /// <summary>
      /// Papeis do usuário separado por ;
      /// </summary>
      /// <value></value>
      string PapeisDoUsuario { get; }


      /// <summary>
      /// Politicas de segurança separada por ;
      /// </summary>
      /// <value></value>
      string PermissoesDoUsuario { get; }

   }
}