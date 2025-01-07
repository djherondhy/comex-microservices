using System.ComponentModel.DataAnnotations;
namespace StockService.Data.Dtos
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "O campo de nome é obrigatório.")]
        public string Name { get; set; }
    }
}