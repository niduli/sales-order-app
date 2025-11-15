using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Customer>> GetAllAsync() => _repo.GetAllAsync();
    }
}
