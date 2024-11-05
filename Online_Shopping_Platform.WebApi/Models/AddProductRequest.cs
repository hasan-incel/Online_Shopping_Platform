using System.ComponentModel.DataAnnotations;

namespace Online_Shopping_Platform.WebApi.Models
{
    public class AddProductRequest
    {
        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}
