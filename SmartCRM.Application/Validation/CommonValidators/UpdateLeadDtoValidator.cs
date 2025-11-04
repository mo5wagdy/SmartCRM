using FluentValidation;
using SmartCRM.Application.Dtos.Lead_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class UpdateLeadDtoValidator : AbstractValidator<UpdateLeadDto>
    {
        public UpdateLeadDtoValidator()
        {
            Include(new CreateLeadDtoValidator());
        }
    }
}
