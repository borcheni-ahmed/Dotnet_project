
using Dotnet_Project.DTOs;
using Dotnet_Project.Entities.Oltp;
using Dotnet_Project.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Dotnet_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Obtenir tous les clients
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();

            var customerDtos = customers.Select(c => new CustomerDto
            {
                CustomerID = c.CustomerID,
                CustomerName = c.CustomerName,
                PhoneNumber = c.PhoneNumber,
                DeliveryAddressLine1 = c.DeliveryAddressLine1,
                DeliveryAddressLine2 = c.DeliveryAddressLine2,
                PostalCode = c.PostalPostalCode,
                StandardDiscountPercentage = c.StandardDiscountPercentage,
                IsOnCreditHold = c.IsOnCreditHold,
                PaymentDays = c.PaymentDays,
                TotalOrders = c.Orders?.Count ?? 0
            });

            return Ok(customerDtos);
        }

        /// <summary>
        /// Obtenir un client par ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound(new { message = $"Client avec ID {id} introuvable" });

            var customerDto = new CustomerDto
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                PhoneNumber = customer.PhoneNumber,
                DeliveryAddressLine1 = customer.DeliveryAddressLine1,
                DeliveryAddressLine2 = customer.DeliveryAddressLine2,
                PostalCode = customer.PostalPostalCode,
                StandardDiscountPercentage = customer.StandardDiscountPercentage,
                IsOnCreditHold = customer.IsOnCreditHold,
                PaymentDays = customer.PaymentDays,
                TotalOrders = customer.Orders?.Count ?? 0
            };

            return Ok(customerDto);
        }

        /// <summary>
        /// Créer un nouveau client
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = new Customer
            {
                CustomerName = customerDto.CustomerName,
                PhoneNumber = customerDto.PhoneNumber,
                DeliveryAddressLine1 = customerDto.DeliveryAddressLine1 ?? "",
                DeliveryAddressLine2 = customerDto.DeliveryAddressLine2,
                PostalPostalCode = customerDto.PostalCode,
                StandardDiscountPercentage = customerDto.StandardDiscountPercentage,
                IsOnCreditHold = customerDto.IsOnCreditHold,
                PaymentDays = customerDto.PaymentDays,
                CustomerCategoryID = 3,
                DeliveryCityID = 19586,
                PostalCityID = 19586,
                PostalAddressLine1 = customerDto.DeliveryAddressLine1 ?? "",
                IsStatementSent = false,
                BillToCustomerID = 1 // ← add this
            };

            var createdCustomer = await _customerRepository.CreateAsync(customer);

            customerDto.CustomerID = createdCustomer.CustomerID;

            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = createdCustomer.CustomerID },
                customerDto);
        }

        /// <summary>
        /// Modifier un client
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (id != customerDto.CustomerID)
                return BadRequest(new { message = "L'ID du client ne correspond pas" });

            if (!await _customerRepository.ExistsAsync(id))
                return NotFound(new { message = $"Client avec ID {id} introuvable" });

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            // Mise à jour des propriétés
            customer.CustomerName = customerDto.CustomerName;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.DeliveryAddressLine1 = customerDto.DeliveryAddressLine1 ?? "";
            customer.DeliveryAddressLine2 = customerDto.DeliveryAddressLine2;
            customer.PostalPostalCode = customerDto.PostalCode;
            customer.StandardDiscountPercentage = customerDto.StandardDiscountPercentage;
            customer.IsOnCreditHold = customerDto.IsOnCreditHold;
            customer.PaymentDays = customerDto.PaymentDays;

            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        /// <summary>
        /// Supprimer un client
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (!await _customerRepository.ExistsAsync(id))
                return NotFound(new { message = $"Client avec ID {id} introuvable" });

            var result = await _customerRepository.DeleteAsync(id);

            if (!result)
                return BadRequest(new { message = "Impossible de supprimer le client" });

            return NoContent();
        }
    }
}