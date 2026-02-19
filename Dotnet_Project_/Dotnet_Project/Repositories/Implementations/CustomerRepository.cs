
using Dotnet_Project.Data.Oltp;
using Dotnet_Project.Entities.Oltp;
using Dotnet_Project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dotnet_Project.Data.Oltp;
using Dotnet_Project.Entities.Oltp;
using Dotnet_Project.Repositories.Interfaces;

namespace Dotnet_Project.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OltpDbContext _context;

        public CustomerRepository(OltpDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .OrderBy(c => c.CustomerName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CustomerID == id);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            // Valeurs par défaut

            customer.LastEditedBy = 1;
            customer.AccountOpenedDate = customer.AccountOpenedDate ?? DateTime.Today; // ← add this
            customer.PostalAddressLine1 = customer.PostalAddressLine1 ?? customer.DeliveryAddressLine1;
            customer.PostalCityID = customer.PostalCityID == 0 ? customer.DeliveryCityID : customer.PostalCityID;
            customer.CustomerCategoryID = customer.CustomerCategoryID == 0 ? 1 : customer.CustomerCategoryID;
            customer.DeliveryMethodID = customer.DeliveryMethodID ?? 1;
            customer.DeliveryPostalCode = customer.DeliveryPostalCode ?? "00000";
            customer.PostalPostalCode = customer.PostalPostalCode ?? "00000";
            customer.WebsiteURL = customer.WebsiteURL ?? "N/A";
            customer.FaxNumber = customer.FaxNumber ?? "N/A";
            customer.PrimaryContactPersonID = customer.PrimaryContactPersonID ?? 1;
            customer.BillToCustomerID = 1; // use an existing valid CustomerID from your DB

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(); // CustomerID is assigned here

            customer.BillToCustomerID = customer.CustomerID; // self-reference
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            customer.LastEditedBy = 1; // Système
            _context.Entry(customer).State = EntityState.Modified;
            _context.Entry(customer).Property(x => x.ValidFrom).IsModified = false;
            _context.Entry(customer).Property(x => x.ValidTo).IsModified = false;
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.CustomerID == id);
        }

        public async Task<int> GetTotalOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerID == customerId)
                .CountAsync();
        }
    }
}