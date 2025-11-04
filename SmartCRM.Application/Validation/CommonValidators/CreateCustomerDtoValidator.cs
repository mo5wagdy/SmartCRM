using FluentValidation;
using SmartCRM.Application.Dtos.Customer_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(25);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
            RuleFor(x => x.CompanyName).MaximumLength(200);
            RuleFor(x => x.CustomerType).NotEmpty().MinimumLength(50);
        }
    }
}
