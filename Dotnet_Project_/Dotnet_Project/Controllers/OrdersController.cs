
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Obtenir toutes les commandes
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderID = o.OrderID,
                CustomerID = o.CustomerID,
                CustomerName = o.Customer?.CustomerName,
                OrderDate = o.OrderDate,
                ExpectedDeliveryDate = o.ExpectedDeliveryDate,
                CustomerPurchaseOrderNumber = o.CustomerPurchaseOrderNumber,
                Comments = o.Comments,
                DeliveryInstructions = o.DeliveryInstructions,
                IsUndersupplyBackordered = o.IsUndersupplyBackordered,
                PickingCompletedWhen = o.PickingCompletedWhen,
                Status = o.PickingCompletedWhen.HasValue ? "Livrée" : "En attente"
            });

            return Ok(orderDtos);
        }

        /// <summary>
        /// Obtenir une commande par ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound(new { message = $"Commande avec ID {id} introuvable" });

            var orderDto = new OrderDto
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                CustomerName = order.Customer?.CustomerName,
                OrderDate = order.OrderDate,
                ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                CustomerPurchaseOrderNumber = order.CustomerPurchaseOrderNumber,
                Comments = order.Comments,
                DeliveryInstructions = order.DeliveryInstructions,
                IsUndersupplyBackordered = order.IsUndersupplyBackordered,
                PickingCompletedWhen = order.PickingCompletedWhen,
                Status = order.PickingCompletedWhen.HasValue ? "Livrée" : "En attente"
            };

            return Ok(orderDto);
        }

        /// <summary>
        /// Obtenir les commandes d'un client
        /// </summary>
        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int customerId)
        {
            if (!await _customerRepository.ExistsAsync(customerId))
                return NotFound(new { message = $"Client avec ID {customerId} introuvable" });

            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderID = o.OrderID,
                CustomerID = o.CustomerID,
                CustomerName = o.Customer?.CustomerName,
                OrderDate = o.OrderDate,
                ExpectedDeliveryDate = o.ExpectedDeliveryDate,
                CustomerPurchaseOrderNumber = o.CustomerPurchaseOrderNumber,
                Comments = o.Comments,
                DeliveryInstructions = o.DeliveryInstructions,
                IsUndersupplyBackordered = o.IsUndersupplyBackordered,
                PickingCompletedWhen = o.PickingCompletedWhen,
                Status = o.PickingCompletedWhen.HasValue ? "Livrée" : "En attente"
            });

            return Ok(orderDtos);
        }

        /// <summary>
        /// Créer une nouvelle commande
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifier que le client existe
            if (!await _customerRepository.ExistsAsync(orderDto.CustomerID))
                return BadRequest(new { message = "Client introuvable" });

            var order = new Order
            {
                CustomerID = orderDto.CustomerID,
                OrderDate = orderDto.OrderDate,
                ExpectedDeliveryDate = orderDto.ExpectedDeliveryDate,
                CustomerPurchaseOrderNumber = orderDto.CustomerPurchaseOrderNumber,
                Comments = orderDto.Comments,
                DeliveryInstructions = orderDto.DeliveryInstructions,
                IsUndersupplyBackordered = orderDto.IsUndersupplyBackordered,
                // Valeurs par défaut obligatoires
                SalespersonPersonID = 1,
                ContactPersonID = 1001,
                LastEditedBy = 1
            };

            var createdOrder = await _orderRepository.CreateAsync(order);

            orderDto.OrderID = createdOrder.OrderID;
            orderDto.Status = "En attente";

            return CreatedAtAction(
                nameof(GetOrder),
                new { id = createdOrder.OrderID },
                orderDto);
        }

        /// <summary>
        /// Modifier une commande
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (id != orderDto.OrderID)
                return BadRequest(new { message = "L'ID de la commande ne correspond pas" });

            if (!await _orderRepository.ExistsAsync(id))
                return NotFound(new { message = $"Commande avec ID {id} introuvable" });

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            // Mise à jour des propriétés
            order.OrderDate = orderDto.OrderDate;
            order.ExpectedDeliveryDate = orderDto.ExpectedDeliveryDate;
            order.CustomerPurchaseOrderNumber = orderDto.CustomerPurchaseOrderNumber;
            order.Comments = orderDto.Comments;
            order.DeliveryInstructions = orderDto.DeliveryInstructions;
            order.IsUndersupplyBackordered = orderDto.IsUndersupplyBackordered;
            order.PickingCompletedWhen = orderDto.PickingCompletedWhen;

            await _orderRepository.UpdateAsync(order);

            return NoContent();
        }

        /// <summary>
        /// Supprimer une commande
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!await _orderRepository.ExistsAsync(id))
                return NotFound(new { message = $"Commande avec ID {id} introuvable" });

            var result = await _orderRepository.DeleteAsync(id);

            if (!result)
                return BadRequest(new { message = "Impossible de supprimer la commande" });

            return NoContent();
        }

        /// <summary>
        /// Marquer une commande comme livrée
        /// </summary>
        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return NotFound(new { message = $"Commande avec ID {id} introuvable" });

            order.PickingCompletedWhen = DateTime.Now;
            await _orderRepository.UpdateAsync(order);

            return Ok(new { message = "Commande marquée comme livrée" });
        }
    }
}