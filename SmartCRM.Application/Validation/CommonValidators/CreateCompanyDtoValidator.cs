using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SmartCRM.Application.Dtos;
using SmartCRM.Application.Dtos.Company_Dtos;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
    {
        CreateCompanyDtoValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        }

    }
}
