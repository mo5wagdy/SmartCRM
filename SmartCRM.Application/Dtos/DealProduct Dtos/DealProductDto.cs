using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.DealProduct_Dtos
{
    public record DealProductDto
    
    (
        int DealId,
        int ProductId,
        int Quantity,
        decimal UnitPrice
    );
}
