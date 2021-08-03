using KikiCourierService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KikiCourierService
{
    class CourierServices
    {
        /// <summary>
        /// Defining offers list
        /// </summary>
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
            int ContinueForMoreOperation;
            do
            {
                Console.WriteLine("Hello! Please enter option of your choice for your operation.\n 1. Calculate Delivery Cost Estimation\n 2. Calculate Delivery Time Estimation");
                int OperationOfChoice = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Base Delivery Cost:");
                int BaseDeliveryCost = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter No of Packages to be delivered:");
                int NoOfPackages = Convert.ToInt32(Console.ReadLine());
                List<Package> packages = new List<Package>();

                //Get all the packages details
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

                if (OperationOfChoice == 1)
                {
                    foreach (Package p in packages)
                    {
                        DeliveryCost cost = GetDeliveryCost(BaseDeliveryCost, p.PackageId, p.PackageWeight, p.Distance, p.OfferCode);
                        Console.WriteLine(cost.PackageId + " " + cost.Discount + " " + cost.TotalCost);
                    }
                }
                else if (OperationOfChoice == 2)
                {
                    Console.WriteLine("Enter No of Vehicles available:");
                    int NoOfVehicles = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter Max Speed of the vehicles:");
                    int MaxSpeed = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter Max carriable weight for the vehicles:");
                    int MaxCarriableWeight = Convert.ToInt32(Console.ReadLine());

                    var packagesList = GetDeliveryTime(packages, NoOfVehicles, MaxSpeed, MaxCarriableWeight);
                    foreach (var p in packagesList)
                    {
                        DeliveryCost cost = GetDeliveryCost(BaseDeliveryCost, p.PackageId, p.PackageWeight, p.Distance, p.OfferCode);
                        Console.WriteLine(cost.PackageId + " " + cost.Discount + " " + cost.TotalCost + " " + p.DeliveryTime);
                    }
                }

                Console.WriteLine("Want to perform more operations?\n 1. Yes\n 2. No");
                ContinueForMoreOperation = Convert.ToInt32(Console.ReadLine());
            } while (ContinueForMoreOperation == 1);
        }

        /// <summary>
        /// Calculate Estimated Delivery Time
        /// </summary>
        /// <param name="packagesList"></param>
        /// <param name="noOfVehicles"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="maxCarriableWeight"></param>
        /// <returns></returns>
        static List<Package> GetDeliveryTime(List<Package> packagesList,int noOfVehicles,int maxSpeed,int maxCarriableWeight)
        {
            //Sorting packages based on weight in ascending and then by Distance in descending order so that for packages with same weight, the later package i.e. package with less distance will be given priority
            var sortedPackages = packagesList.OrderBy(p => p.PackageWeight).ThenByDescending(p => p.Distance).ToList();

            var vehiclesList = new List<Vehicle>();
            int index = 1;
            while (index <= noOfVehicles)
            {
                vehiclesList.Add(new Vehicle { VehicleId = index });
                index++;
            }

            int noOfPackagesDelivered = 0;

            int front = 0;
            int end = front;
            while (noOfPackagesDelivered < sortedPackages.Count())
            {
                float carryingWeight = 0;

                //To get list of packages to be delivered in one one delivery cycle
                for (int i = 0; i < sortedPackages.Count(); i++)
                {
                    if (sortedPackages[i].DeliveryTime == null)
                    {
                        if ((carryingWeight + sortedPackages[i].PackageWeight) <= maxCarriableWeight)
                        {
                            carryingWeight += sortedPackages[i].PackageWeight;
                            end = i;
                        }
                        else if (((carryingWeight + sortedPackages[i].PackageWeight) - sortedPackages[front].PackageWeight) <= maxCarriableWeight)
                        {
                            carryingWeight = (carryingWeight + sortedPackages[i].PackageWeight) - sortedPackages[front].PackageWeight;
                            end = i;
                            while (true)
                            {
                                front = front + 1;
                                if (sortedPackages[front].DeliveryTime == null) break;
                            }
                        }
                    }
                }

                //Sorting vehicles so that the vehicle with least consumed time will be used for the cycle
                vehiclesList = SortVehiclesBasedOnBaseStartTime(vehiclesList);

                float maxDistanceToBeCovered = 0;

                //Calculate delivery time for list of packages in the cycle
                while (front <= end)
                {
                    if (sortedPackages[front].DeliveryTime == null)
                    {
                        sortedPackages[front].DeliveryTime = Math.Round(vehiclesList[0].BaseDeliveryStartTime + (Math.Truncate(100 * (sortedPackages[front].Distance / maxSpeed)) / 100), 2);
                        maxDistanceToBeCovered = maxDistanceToBeCovered < sortedPackages[front].Distance ? sortedPackages[front].Distance : maxDistanceToBeCovered;
                        noOfPackagesDelivered++;
                    }
                    front++;
                }
                vehiclesList[0].BaseDeliveryStartTime = (vehiclesList[0].BaseDeliveryStartTime + ((Math.Truncate(100 * (maxDistanceToBeCovered / maxSpeed)) / 100) * 2));
                foreach (var v in vehiclesList)
                {
                }
                front = 0;
                end = 0;
            }

            return sortedPackages.OrderBy(p=>p.PackageId).ToList();
        }

        /// <summary>
        /// Sort Vehicles based on consumed delivery time for packages
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns></returns>
        static List<Vehicle> SortVehiclesBasedOnBaseStartTime(List<Vehicle> vehicles)
        {
            return vehicles.OrderBy(v => v.BaseDeliveryStartTime).ThenBy(v => v.VehicleId).ToList();
        }

        /// <summary>
        /// Calculate Estimated Discount & Total Cost for packages based on applied offer & validating against pre-defined offers
        /// </summary>
        /// <param name="BaseDeliveryCost"></param>
        /// <param name="packageId"></param>
        /// <param name="weight"></param>
        /// <param name="distance"></param>
        /// <param name="offerCode"></param>
        /// <returns></returns>
        static DeliveryCost GetDeliveryCost(int BaseDeliveryCost,string packageId,int weight,float distance,string offerCode)
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
