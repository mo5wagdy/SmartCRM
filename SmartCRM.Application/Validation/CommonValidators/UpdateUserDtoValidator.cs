using FluentValidation;
using SmartCRM.Application.Dtos.User_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Phone).MaximumLength(25);
            RuleFor(x => x.Role).NotEmpty();
            RuleFor(x => x.Status).NotEmpty();
            RuleFor(x => x.ImagePath).MaximumLength(500);
        }
    }
}
