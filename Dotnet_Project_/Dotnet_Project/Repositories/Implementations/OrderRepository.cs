
using Dotnet_Project.Data.Oltp;
using Dotnet_Project.Entities.Oltp;
using Dotnet_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dotnet_Project.Data.Oltp;
using Dotnet_Project.Entities.Oltp;
using Dotnet_Project.Repositories.Interfaces;

namespace Dotnet_Project.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OltpDbContext _context;

        public OrderRepository(OltpDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.CustomerID == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            // Valeurs par défaut
            order.LastEditedBy = 1; // Système
            order.LastEditedWhen = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            order.LastEditedBy = 1; // Système
            order.LastEditedWhen = DateTime.Now;

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.OrderID == id);
        }
    }
}