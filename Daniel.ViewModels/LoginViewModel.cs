using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.ViewModels
{
    public class LoginViewModel:DanielViewModel
    {
        public string NomeUsuario { get; set; }
        public string SenhaUsuario { get; set; }
    
        public int? CodigoVerificacao { get; set; }
        public string ConfirmarSenha {get;set;}
        public string SiglaIdioma { get; set; }

        //Utilizado no Esqueci a Senha
        public string UrlOrigem {get;set;}

        public string Validador{get;set;}
    }
}
