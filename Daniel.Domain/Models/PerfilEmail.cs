using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.Domain.Models
{
    public class PerfilEmail : BaseModel
    {
        public string Descricao { get; set; }
        public string NomeConta { get; set; }
        public string NomeExibicao { get; set; }
        public string Email { get; set; }
        public string EmailResposta { get; set; }
        public string Servidor { get; set; }
        public string TipoServidor { get; set; }
        public int Porta { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool AutenticacaoSSL { get; set; }
        public string EmailCopia { get; set; }
        public string EmailCopiaOculta { get; set; }
    }
}
