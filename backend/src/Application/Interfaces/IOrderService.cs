using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<SalesOrder> CreateOrderAsync(SalesOrder order);
        Task<SalesOrder> UpdateOrderAsync(SalesOrder order);
        Task<SalesOrder?> GetOrderByIdAsync(int id);
        Task<IEnumerable<SalesOrder>> GetAllOrdersAsync();
    }
}
