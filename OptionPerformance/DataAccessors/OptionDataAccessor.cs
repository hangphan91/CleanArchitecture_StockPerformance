using System;
using System.Dynamic;
using OptionPerformance.DataAccessors.Models;
namespace OptionPerformance.DataAccessors
{
    public class OptionDataAccessor
    {
        public static BatchedOptionsData GetOptionsData(string symbol)
        {
            var batchedOptionsData = new BatchedOptionsData(symbol);
            // call api to get Option info
            // call stock performance to get stock performance info, price, list of options, greeks

            // map to optionsData and return

            return batchedOptionsData;
        }
    }
}

