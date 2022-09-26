namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
