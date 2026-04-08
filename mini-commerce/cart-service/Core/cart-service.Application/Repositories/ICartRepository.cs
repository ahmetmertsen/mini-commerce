using cart_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(Guid id);
        Task AddAsync(Cart cart);
        void Update(Cart cart);
        Task SaveChangesAsync();

        Task<Cart?> GetByIdWithItemsAsync(Guid id);
        Task<Cart?> GetByCustomerIdWithItemsAsync(Guid customerId);
    }
}
