using FluentValidation;
using SmartCRM.Application.Dtos.DealProduct_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Validation.CommonValidators
{
    public class CreateDealProductDtoValidator :  AbstractValidator<CreateDealProductDto>
    {
        public CreateDealProductDtoValidator()
        {
            RuleFor(x => x.DealId).GreaterThan(0);
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
