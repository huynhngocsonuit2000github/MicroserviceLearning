using Ordering.Api.Entity;
using Ordering.Api.Model;

namespace Ordering.Api.Services
{
    public interface IOrderService
    {
        Task<Order> Create(OrderRequest request);
        Task<List<Order>> GetAll();
        Task<Order?> GetById(int id);
    }
}