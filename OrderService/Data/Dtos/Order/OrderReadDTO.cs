using OrderService.Enums;

namespace OrderService.DTOs.Order
{
    public class OrderReadDTO
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public ICollection<OrderItemReadDTO> OrderItems { get; set; }
    }

}
