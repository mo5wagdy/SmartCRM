using FluentValidation;
using SmartCRM.Application.Dtos.Deal_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateDealDtoValidator : AbstractValidator<CreateDealDto>
    {
        public CreateDealDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.CustomerId).GreaterThan(0);
        }
    }
}
