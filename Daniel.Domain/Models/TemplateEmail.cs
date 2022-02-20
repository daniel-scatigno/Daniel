using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.Domain.Models
{
    public class TemplateEmail : BaseModel
    {
        public string Descricao { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
    }
}
