using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }

        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal LineTotal => UnitPrice * Quantity;

        public Cart? Cart { get; set; }

        public CartItem() { }

        public CartItem(Guid productId, Guid productVariantId, string productName, decimal unitPrice, int quantity)
        {
            if (productId == Guid.Empty)
                throw new ArgumentException("Geçerli bir ürün bilgisi girilmelidir.");

            if (productVariantId == Guid.Empty)
                throw new ArgumentException("Geçerli bir ürün varyantı seçilmelidir.");

            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Ürün adı boş olamaz.");

            if (unitPrice <= 0)
                throw new ArgumentException("Ürün fiyatı sıfırdan büyük olmalıdır.");

            if (quantity <= 0)
                throw new ArgumentException("Ürün adedi sıfırdan büyük olmalıdır.");

            Id = Guid.NewGuid();
            ProductId = productId;
            ProductVariantId = productVariantId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
