using System;
using EntityPersistence.DataAccessors;
using StockPerformance_CleanArchitecture.Managers;
using StockPerformanceCalculator.DatabaseAccessors;

namespace StockPerformance_CleanArchitecture.Helpers
{
	public static class ManagerHelper
	{
        private static SearchDetailManager _searchDetailManager;
        public static SearchDetailManager SearchDetailManager
           = GetInstance();

        private static SearchDetailManager GetInstance()
        {
            if (_searchDetailManager == null)
            {
                _searchDetailManager = new SearchDetailManager();
            }
            return _searchDetailManager;
        }
    }
}

