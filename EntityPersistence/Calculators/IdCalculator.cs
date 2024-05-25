using System.Collections.Concurrent;
using EntityDefinitions;

namespace EntityPersistence.Calculators
{
    public class IdCalculator
    {
        public List<PerformanceIdHub> IdManagers { get; set; }

        public IdCalculator()
        {
        }

        internal long GetNewId(long id)
        {
            return id + 1;
        }
    }
}

