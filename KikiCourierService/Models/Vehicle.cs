using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KikiCourierService.Models
{
    class Vehicle
    {
        public int VehicleId { get; set; }
        public double BaseDeliveryStartTime { get; set; }
        public Vehicle()
        {
            BaseDeliveryStartTime = 0d;
        }
    }
}
