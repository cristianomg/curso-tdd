using NerdStore.Vendas.Core.Data;

namespace NerdStore.Vendas.Domain.Interfaces
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        void Adicionar(Pedido pedido);
        void AdicionarItem(PedidoItem pedido);
        void Atualizar(Pedido pedido);
        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);
        void AtualizarItem(PedidoItem pedidoItem);
    }
}
