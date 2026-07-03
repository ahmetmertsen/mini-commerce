using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.UpdateVariantStock
{
    public class UpdateProductVariantStockCommand : IRequest<UpdateProductVariantStockCommandResponse>
    {
        public Guid VariantId { get; set; }
        public int StockQuantity { get; set; }
    }
}
