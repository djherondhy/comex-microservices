namespace OrderService.Data.Dtos.RabbitMq
{
    public class UpdateProductQuantityInStockDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int OrderId  { get; set; }
    }
}