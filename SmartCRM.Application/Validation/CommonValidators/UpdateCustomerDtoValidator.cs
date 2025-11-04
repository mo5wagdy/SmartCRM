using FluentValidation;
using SmartCRM.Application.Dtos.Customer_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            Include(new CreateCustomerDtoValidator());
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}
