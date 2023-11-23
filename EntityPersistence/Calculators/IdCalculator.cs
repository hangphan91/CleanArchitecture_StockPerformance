using EntityDefinitions;

namespace EntityPersistence.Calculators
{
    public class IdCalculator
	{
		DataAccessors.DataContext _dataContext;
        public List<PerformanceIdHub> IdManagers { get; set; }

        public IdCalculator(DataAccessors.DataContext dataContext)
		{
			_dataContext = dataContext;
		}

        internal long GetNewId(IEnumerable<long> ids)
        {
            return ids.Max(d => d) + 1;
        }
    }
}

