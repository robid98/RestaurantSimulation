using RestaurantSimulation.Application.Common.Interfaces.Persistence;

namespace RestaurantSimulation.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
            private RestaurantSimulationContext _dbContext;

            public UnitOfWork(RestaurantSimulationContext context)
            {
                _dbContext = context;
            }

            public Task SaveChangesAsync()
            {
                return _dbContext.SaveChangesAsync();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_dbContext != null)
                    {
                        _dbContext.Dispose();
                        _dbContext = null!;
                    }
                }
            }
    }
}
