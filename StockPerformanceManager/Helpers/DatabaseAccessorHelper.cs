using System;
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
                var context = new DataContext();
                EntityDefinitionsAccessor = new PerformanceDataAccessor(context);
            }
            return EntityDefinitionsAccessor;
        }


    }
}

