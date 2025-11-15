using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<SalesOrder> CreateOrderAsync(SalesOrder order)
        {
            CalculateOrderTotals(order);
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<SalesOrder> UpdateOrderAsync(SalesOrder order)
        {
            CalculateOrderTotals(order);
            return await _orderRepository.UpdateAsync(order);
        }

        public Task<SalesOrder?> GetOrderByIdAsync(int id)
        {
            return _orderRepository.GetByIdAsync(id); // Includes Customer + Lines
        }

        public Task<IEnumerable<SalesOrder>> GetAllOrdersAsync()
        {
            return _orderRepository.GetAllAsync(); // Includes Customer
        }

        private void CalculateOrderTotals(SalesOrder order)
        {
            decimal totalExcl = 0;
            decimal totalTax = 0;
            decimal totalIncl = 0;

            foreach (var line in order.Lines)
            {
                line.ExclAmount = line.Price * line.Quantity;
                line.TaxAmount = line.ExclAmount * (line.TaxRate / 100);
                line.InclAmount = line.ExclAmount + line.TaxAmount;

                totalExcl += line.ExclAmount;
                totalTax += line.TaxAmount;
                totalIncl += line.InclAmount;
            }

            order.TotalExcl = totalExcl;
            order.TotalTax = totalTax;
            order.TotalIncl = totalIncl;

            // Keep original date if editing
            if (order.OrderDate == default)
                order.OrderDate = DateTime.Now;
        }
    }
}
