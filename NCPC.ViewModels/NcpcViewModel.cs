using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCPC.ViewModels
{
    public class NcpcViewModel : IValidatableObject
    {
        [Display(Name = "Código")]
        public virtual int Id { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Exemplo de validação
            // if (Genre == Genre.Classic && ReleaseDate.Year > _classicYear)
            // {
            //    yield return new ValidationResult(
            //        $"Classic movies must have a release year no later than {_classicYear}.",
            //        new[] { nameof(ReleaseDate) });
            // }
            yield break;
        }

    }
}

