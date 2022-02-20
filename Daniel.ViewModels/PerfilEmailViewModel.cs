using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Daniel.ViewModels
{
    public class PerfilEmailViewModel : DanielViewModel
    {


        [Display(Name = "Descricao")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string Descricao { get; set; }

        [Display(Name = "NomeConta")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string NomeConta { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string Email { get; set; }

        [Display(Name = "NomeExibicao")]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string NomeExibicao { get; set; }

        [Display(Name = "EmailResposta")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string EmailResposta { get; set; }

         [Display(Name = "Servidor")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string Servidor { get; set; }

        [Display(Name = "TipoServidor")]        
        [StringLength(20,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string TipoServidor { get; set; }

        [Display(Name = "Porta")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        public virtual int Porta { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string Usuario { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessageResourceName = "RequiredError")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string Senha { get; set; }

        [Display(Name = "AutenticacaoSSL")]        
        public virtual bool AutenticacaoSSL { get; set; }

        [Display(Name = "EmailCopia")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string EmailCopia { get; set; }

        [Display(Name = "EmailCopiaOculta")]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength")]
        public virtual string EmailCopiaOculta { get; set; }

    }
}
