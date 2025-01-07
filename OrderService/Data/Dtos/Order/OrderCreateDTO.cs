using OrderService.Enums;

namespace OrderService.DTOs.Order
{
    public class OrderCreateDTO
    {
        public OrderStatus Status { get; set; }
        public ICollection<OrderItemCreateDTO> OrderItems { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
