using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.Order.Dtos
{
    public class AddOrderDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public int Id { get; set; }
        public List<int> PorductIds { get; set; }
    }
}
