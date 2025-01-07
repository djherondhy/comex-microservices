namespace OrderService.DTOs.Order
{
    public class OrderItemReadDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public string NameProduct { get; set; }

        public string ImageProduct { get; set; }

    }
}
