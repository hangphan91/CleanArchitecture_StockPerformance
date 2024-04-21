using System;
namespace StockPerformance_CleanArchitecture.Models.ProfitDetails
{
    public class GrowthSpeed
    {
        public decimal Rate { get; set; }
        public GrowthSpeed()
        {
        }

        public virtual string DisplayGrowthSpeed()
        {
            if (Rate > 0)
                return $"Growth rate is {Rate}.";

            return $"Growth rate is negative {Rate}.";
        }
    }
}

