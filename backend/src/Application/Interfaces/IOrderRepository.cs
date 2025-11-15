using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<SalesOrder?> GetByIdAsync(int id);
        Task<IEnumerable<SalesOrder>> GetAllAsync();
        Task<SalesOrder> CreateAsync(SalesOrder order);
        Task<SalesOrder> UpdateAsync(SalesOrder order);
    }
}
