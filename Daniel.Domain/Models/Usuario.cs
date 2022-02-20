using System;
namespace Daniel.Domain.Lib.Models
{
   public interface IUsuario
   {
      string NomeUsuario { get; set; }

      string Email { get; set; }

      string Senha { get; set; }

      DateTime DataCriacao { get; set; }

      bool Ativo { get; set; }

   }
}