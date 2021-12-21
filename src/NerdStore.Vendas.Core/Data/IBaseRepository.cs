using NerdStore.Vendas.Core.DomainObjects;

namespace NerdStore.Vendas.Core.Data
{
    public interface IBaseRepository<T> : IDisposable
        where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
