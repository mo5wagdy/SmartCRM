using FluentValidation;
using SmartCRM.Application.Dtos.Note_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteDtoValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.RelatedTo).NotEmpty();
        }
    }
}
