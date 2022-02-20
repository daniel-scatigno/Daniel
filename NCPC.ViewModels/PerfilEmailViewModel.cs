using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCPC.Resource.Resources;


namespace NCPC.ViewModels
{
    public class PerfilEmailViewModel : NcpcViewModel
    {


        [Display(Name = "Descricao", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string Descricao { get; set; }

        [Display(Name = "NomeConta", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string NomeConta { get; set; }

        [Display(Name = "Email", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string Email { get; set; }

        [Display(Name = "NomeExibicao", ResourceType = typeof(SfResources))]
        [StringLength(100,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string NomeExibicao { get; set; }

        [Display(Name = "EmailResposta", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string EmailResposta { get; set; }

         [Display(Name = "Servidor", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string Servidor { get; set; }

        [Display(Name = "TipoServidor", ResourceType = typeof(SfResources))]        
        [StringLength(20,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string TipoServidor { get; set; }

        [Display(Name = "Porta", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        public virtual int Porta { get; set; }

        [Display(Name = "Usuario", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string Usuario { get; set; }

        [Display(Name = "Senha", ResourceType = typeof(SfResources))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string Senha { get; set; }

        [Display(Name = "AutenticacaoSSL", ResourceType = typeof(SfResources))]        
        public virtual bool AutenticacaoSSL { get; set; }

        [Display(Name = "EmailCopia", ResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string EmailCopia { get; set; }

        [Display(Name = "EmailCopiaOculta", ResourceType = typeof(SfResources))]
        [StringLength(300,ErrorMessageResourceName = "MaximumStringLength", ErrorMessageResourceType = typeof(SfResources))]
        public virtual string EmailCopiaOculta { get; set; }

    }
}
