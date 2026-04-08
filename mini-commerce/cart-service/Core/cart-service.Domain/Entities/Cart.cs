using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        private Cart() { }

        public Cart(Guid customerId, List<CartItem> cartItems)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Geçerli bir müşteri bilgisi girilmelidir.");

            if (cartItems is null || cartItems.Count == 0)
                throw new ArgumentException("Sepet en az bir ürün içermelidir.");

            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedDate = DateTime.UtcNow;

            foreach (var cartItem in cartItems)
            {
                CartItems.Add(cartItem);
            }

            TotalAmount = CartItems.Sum(x => x.LineTotal);
        }
    }
}
