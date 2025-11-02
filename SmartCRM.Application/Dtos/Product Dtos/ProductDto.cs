using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCRM.Application.Dtos.Product_Dtos
{
    public record ProductDto
    (
        int ProductId,
        string Name,
        string Description,
        decimal Price,
        int QuantityInStock,
        DateTime CreatedAt
    );
}
