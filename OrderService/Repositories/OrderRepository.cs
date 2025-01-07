
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Enums;
using OrderService.Models;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;


        public OrderRepository(OrderContext context)
        {
            _context = context;

        }

        public async Task CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task DeleteOrderById(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Order> UpdateStatusOrderById(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");
            }

            order.Status = status;

            return order;
        }
    }
}