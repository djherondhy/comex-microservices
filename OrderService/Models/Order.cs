
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderService.Enums;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public OrderStatus Status { get; set; } = OrderStatus.Pendente;

        public double Total { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
