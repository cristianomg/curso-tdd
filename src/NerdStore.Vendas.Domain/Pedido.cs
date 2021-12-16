using NerdStore.Vendas.Core;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public static readonly int MAX_UNIDADES_ITEM_PEDIDO = 15;
        public static readonly int MIN_UNIDADES_ITEM_PEDIDO = 1;
        protected Pedido()
        {

        }
        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }

        private readonly List<PedidoItem> _pedidoItems = new List<PedidoItem>();
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public PedidoStatus Status { get; private set; }

        public void AdicionarItem(PedidoItem pedidoItem) 
        {
            var pedidoExistente = _pedidoItems.FirstOrDefault(x => x.ProdutoId == pedidoItem.ProdutoId);

            if (pedidoExistente is null)
                _pedidoItems.Add(pedidoItem);
            else
                pedidoExistente.AdicionarUnidades(pedidoItem.Quantidade);

            CalcularValorTotal();
        }

        private void CalcularValorTotal()
        {
            ValorTotal = PedidoItems.Sum(x => x.CalcularValor());

        }

        public void TornarRascunho()
        {
            Status = PedidoStatus.Rascunho;
        }
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}
