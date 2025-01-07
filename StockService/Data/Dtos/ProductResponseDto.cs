namespace StockService.Data.Dtos;
public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int AvailableQuantity { get; set; }
    public string image { get; set; }
    public int CategoryId { get; set; }
}
