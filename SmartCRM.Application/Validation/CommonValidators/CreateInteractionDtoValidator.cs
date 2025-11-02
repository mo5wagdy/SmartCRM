using FluentValidation;
using SmartCRM.Application.Dtos.Interaction_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateInteractionDtoValidator : AbstractValidator<CreateInteractionDto>
    {
        public CreateInteractionDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.CreatedBy).GreaterThan(0);
        }
    }
}
