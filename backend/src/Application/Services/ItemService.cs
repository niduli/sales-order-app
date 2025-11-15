using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repo;

        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Item>> GetAllAsync() => _repo.GetAllAsync();
    }
}
