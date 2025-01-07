using System.ComponentModel.DataAnnotations;
namespace StockService.Data.Dtos
{
    public class ReadCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}