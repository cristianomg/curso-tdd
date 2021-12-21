namespace NerdStore.Vendas.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
