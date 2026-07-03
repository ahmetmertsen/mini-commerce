using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Dtos
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
