using FluentValidation;
using SmartCRM.Application.Dtos.Interaction_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class UpdateInteractionDtoValidator : AbstractValidator<UpdateInteractionDto>
    {
        public UpdateInteractionDtoValidator()
        {
            Include(new CreateInteractionDtoValidator());
        }
    }
}
