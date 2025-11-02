using FluentValidation;
using SmartCRM.Application.Dtos.Lead_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateLeadDtoValidator : AbstractValidator<CreateLeadDto>
    {
        public CreateLeadDtoValidator()
        {
            RuleFor(x => x.ContactName).NotEmpty().MaximumLength(200)
        }
    }
}
