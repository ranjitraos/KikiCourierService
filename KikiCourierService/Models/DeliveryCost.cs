using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikiCourierService.Models
{
    class DeliveryCost
    {
        public string PackageId { get; set; }
        public float Discount { get; set; }
        public float TotalCost { get; set; }
    }
}
