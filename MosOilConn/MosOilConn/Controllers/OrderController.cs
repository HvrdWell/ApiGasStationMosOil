using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MosOilConn.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MosOilConn
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly TestdbContext TestdbContext;

        public OrderController(TestdbContext TestdbContext)
        {
            this.TestdbContext = TestdbContext;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDTO orderDto)
        {
            // Validate the incoming order data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new Order entity and map the data from the orderDto
            var order = new Order
            {
                OrderId = orderDto.OrderId,
                IdColumns = orderDto.IdColumns,
                IdUser = orderDto.IdUser,
                Status = orderDto.Status,
                TotalPrice = orderDto.TotalPrice
            };
            // Add the order to the DbContext and save changes to the database
            TestdbContext.Orders.Add(order);
            TestdbContext.SaveChanges();

            return Ok();
        }
    }
}

