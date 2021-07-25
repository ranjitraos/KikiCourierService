using System;
using System.Collections.Generic;

namespace KikiCourierService
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

    class Package
    {
        public string PackageId { get; set; }
        public int PackageWeight { get; set; }
        public int Distance { get; set; }
        public string OfferCode { get; set; }
    }

    class DeliveryCost
    {
        public string PackageId { get; set; }
        public float Discount { get; set; }
        public float TotalCost { get; set; }
    }

    class Program
    {
        private static readonly List<Offer> OffersList = new List<Offer>() {
                new Offer
                {
                    Code = "OFR001",
                    DiscountPercentage = 10f,
                    MinDistance = 0,
                    MaxDistance = 200,
                    MinWeight = 70,
                    MaxWeight = 200
                },
                new Offer()
                {
                    Code = "OFR002",
                    DiscountPercentage = 7f,
                    MinDistance = 50,
                    MaxDistance = 150,
                    MinWeight = 100,
                    MaxWeight = 250
                },
                new Offer()
                {
                    Code = "OFR003",
                    DiscountPercentage = 5f,
                    MinDistance = 50,
                    MaxDistance = 250,
                    MinWeight = 10,
                    MaxWeight = 150
                }
            };
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Enter Base Delivery Cost:");
            int BaseDeliveryCost = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter No of Packages to be delivered:");
            int NoOfPackages = Convert.ToInt32(Console.ReadLine());
            List<Package> packages = new List<Package>();

            while (NoOfPackages > 0)
            {
                Console.WriteLine("Enter Package PackageID, PackageWeight, Distance and OfferCode (Followed by enter key after each input)):");
                packages.Add(new Package
                {
                    PackageId = Console.ReadLine(),
                    PackageWeight = Convert.ToInt32(Console.ReadLine()),
                    Distance = Convert.ToInt32(Console.ReadLine()),
                    OfferCode = Console.ReadLine()
                });
                NoOfPackages--;
            }

            foreach(Package p in packages)
            {
                DeliveryCost cost = GetDeliveryCost(BaseDeliveryCost, p.PackageId, p.PackageWeight, p.Distance, p.OfferCode);
                Console.WriteLine(cost.PackageId + " " + cost.Discount + " " + cost.TotalCost);
            }
        }

        static DeliveryCost GetDeliveryCost(int BaseDeliveryCost,string packageId,int weight,int distance,string offerCode)
        {
            float totalCost = BaseDeliveryCost + (weight * 10) + (distance * 5);
            float discount = 0;

            Offer offerObj = OffersList.Find(o => String.Equals(o.Code, offerCode));
            if (offerObj != null)
            {
                if (weight >= offerObj.MinWeight && weight <= offerObj.MaxWeight && distance >= offerObj.MinDistance && distance <= offerObj.MaxDistance)
                {
                    discount = totalCost * (offerObj.DiscountPercentage / 100);
                    totalCost -= discount;
                }
            }

            return new DeliveryCost { PackageId = packageId, Discount = discount, TotalCost = totalCost };
        }
    }
}
