using Microsoft.EntityFrameworkCore;
using Ordering.Api.Data;
using Ordering.Api.Entity;
using Ordering.Api.Model;

namespace Ordering.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders
                .Include(e => e.OrderItems)
                .ToListAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            return await _context.Orders
                .Include(e => e.OrderItems)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Order> Create(OrderRequest request)
        {
            var order = new Order()
            {
                CreatedDate = DateTime.Now,
                UserId = request.UserId,
                FinalPrice = request.OrderItems.Sum(e => e.Quantity * e.FinalPrice),
                OriginalPrice = request.OrderItems.Sum(e => e.Quantity * e.OriginalPrice),
                OrderItems = request.OrderItems.Select(e => new OrderItem()
                {
                    FinalPrice = e.FinalPrice,
                    OriginalPrice = e.OriginalPrice,
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    Quantity = e.Quantity
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
