using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikiCourierService.Models
{
    class Offer
    {
        public string Code { get; set; }
        public float DiscountPercentage { get; set; }
        public int MinDistance { get; set; }
        public int MaxDistance { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
    }
}
