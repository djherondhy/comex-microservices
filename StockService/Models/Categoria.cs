using System.ComponentModel.DataAnnotations;

namespace StockService.Models;

public class Categoria
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Name { get; set; }

    public virtual ICollection<Produto> Products { get; set; }
}
