
using OrderService.Enums;
using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task SaveChangesAsync();
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<Order> UpdateStatusOrderById(int id, OrderStatus Status);
        Task DeleteOrderById(Order order);
    }
}
