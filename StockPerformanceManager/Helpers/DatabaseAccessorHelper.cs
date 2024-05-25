using System;
using System.Collections.Concurrent;
using DocumentFormat.OpenXml.Drawing.Charts;
using EntityPersistence.DataAccessors;
using StockPerformanceCalculator.DatabaseAccessors;

namespace StockPerformance_CleanArchitecture.Helpers
{
    public static class DatabaseAccessorHelper
    {
        public static IEntityDefinitionsAccessor EntityDefinitionsAccessor
            = GetDataAccessorInstance();

        private static IEntityDefinitionsAccessor GetDataAccessorInstance()
        {
            if (EntityDefinitionsAccessor == null)
            {
                EntityDefinitionsAccessor = new PerformanceDataAccessor();
            }
            return EntityDefinitionsAccessor;
        }


    }
}

