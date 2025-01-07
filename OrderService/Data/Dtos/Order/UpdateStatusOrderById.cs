using OrderService.Enums;

namespace OrderService.DTOs.Order;
public class UpdateStatusOrderByIdDto
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}
