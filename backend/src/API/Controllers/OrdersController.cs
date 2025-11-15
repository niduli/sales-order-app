using Application.Interfaces;
using AutoMapper;
using API.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            var result = orders.Select(o => new
            {
                id = o.Id,
                customerName = o.Customer != null ? o.Customer.Name : "",
                totalIncl = o.TotalIncl,
                
                // Always return InvoiceDate (not OrderDate)
                invoiceDate = o.InvoiceDate
            });

            return Ok(result);
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = _mapper.Map<SalesOrder>(dto);

            // Ensure InvoiceDate is set
            if (order.InvoiceDate == default)
                order.InvoiceDate = DateTime.Now;

            var created = await _orderService.CreateOrderAsync(order);

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto dto)
        {
            var existing = await _orderService.GetOrderByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);

            var updated = await _orderService.UpdateOrderAsync(existing);

            return Ok(updated);
        }
    }
}
