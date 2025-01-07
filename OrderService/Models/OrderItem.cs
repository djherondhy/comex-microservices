using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class OrderItem
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que 0")]
        public int Amount { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public double Total => Amount * UnitPrice;

        [Required]
        public string NameProduct { get; set; }

        [Required]
        public string ImageProduct { get; set; }

        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
