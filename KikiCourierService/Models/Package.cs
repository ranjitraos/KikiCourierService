using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikiCourierService.Models
{
    class Package
    {
        public string PackageId { get; set; }
        public int PackageWeight { get; set; }
        public float Distance { get; set; }
        public string OfferCode { get; set; }
        public double? DeliveryTime { get; set; }
    }
}
