using FluentValidation;
using SmartCRM.Application.Dtos.Deal_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class UpdateDealDtoValidator : AbstractValidator<UpdateDealDto>
    {
        public UpdateDealDtoValidator()
        {
            Include(new CreateDealDtoValidator());
        }
    }
}
