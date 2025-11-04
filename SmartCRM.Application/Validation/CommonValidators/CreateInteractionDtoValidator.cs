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
            RuleFor(x => x.Description).MaximumLength(1000);
            RuleFor(x => x.ImteractionType).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
            RuleFor(x => x.RelatedTo).NotEmpty();
            RuleFor(x => x.CreatedBy).GreaterThan(0);
        }
    }
}
