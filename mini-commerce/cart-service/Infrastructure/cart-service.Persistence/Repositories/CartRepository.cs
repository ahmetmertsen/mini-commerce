using cart_service.Application.Repositories;
using cart_service.Domain.Entities;
using cart_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartServiceDbContext _context;

        public CartRepository(CartServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByIdAsync(Guid id) => await _context.Carts.FindAsync(id);

        public async Task AddAsync(Cart cart) => await _context.Carts.AddAsync(cart);

        public void Update(Cart cart) => _context.Carts.Update(cart);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<Cart?> GetByIdWithItemsAsync(Guid id) =>
            await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cart?> GetByCustomerIdWithItemsAsync(Guid customerId) =>
            await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
