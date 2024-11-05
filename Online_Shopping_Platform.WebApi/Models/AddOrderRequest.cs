using System.ComponentModel.DataAnnotations;

namespace Online_Shopping_Platform.WebApi.Models
{
    public class AddOrderRequest
    {
        [Required]
        public DateTime OrderDate { get; set; }= DateTime.Now;
        [Required]
        public decimal TotalAmount { get; set; }
        
        public List<int> PorductIds { get; set; }
    }
}
