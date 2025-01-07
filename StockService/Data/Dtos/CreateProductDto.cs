using System.ComponentModel.DataAnnotations;

namespace StockService.Data.Dtos;

public class CreateProductDto
{
    [Required(ErrorMessage = "O Nome do produto é obrigatório")]
    [MaxLength(100, ErrorMessage = "O tamanho do nome não pode exceder 100 caracteres")]
    public string Name { get; set; }

    [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
    public string Description { get; set; }

    [Required(ErrorMessage = "O Preço do produto é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0")]
    public float Price { get; set; }

    [Required(ErrorMessage = "A quantidade do produto é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0")]
    public int AvailableQuantity { get; set; }

    [Required(ErrorMessage = "A imagem do produto é obrigatório")]
    public string image { get; set; }

    [Required(ErrorMessage = "O campo de CategoriaId é obrigatório.")]
    public int CategoryId { get; set; }
}