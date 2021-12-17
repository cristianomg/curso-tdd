using FluentValidation.Results;
using NerdStore.Vendas.Core;
using NerdStore.Vendas.Domain.Strategy.DescontoVoucher;

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

        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }
        public decimal Desconto { get; set; }

        public void AdicionarItem(PedidoItem pedidoItem)
        {

            if (PedidoItemExistente(pedidoItem))
            {
                var pedidoExistente = _pedidoItems.First(x => x.ProdutoId == pedidoItem.ProdutoId);
                pedidoExistente.AdicionarUnidades(pedidoItem.Quantidade);

            }
            else
            {
                _pedidoItems.Add(pedidoItem);
            }
            CalcularValorTotal();
        }

        private void CalcularValorDesconto()
        {
            if (!VoucherUtilizado) return;

            var desconto = AbstractDesconto
                .Factory(Voucher.TipoDescontoVoucher)
                .CalcularDesconto(ValorTotal, Voucher);

            ValorTotal = desconto > ValorTotal ? 0 : ValorTotal - desconto;
            Desconto = desconto;
        }

        private void CalcularValorTotal()
        {

            ValorTotal = PedidoItems.Sum(x => x.CalcularValor());

            CalcularValorDesconto();
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
        private bool PedidoItemExistente(PedidoItem pedidoItem)
        {
            return _pedidoItems.Any(x => x.ProdutoId == pedidoItem.ProdutoId);
        }

        private void ValidarPedidoItemExistente(PedidoItem pedidoItem)
        {
            if (!PedidoItemExistente(pedidoItem))
                throw new DomainException("O item não existe no pedido.");
        }

        public void AtualizarItem(PedidoItem pedidoItemAtualizado)
        {
            ValidarPedidoItemExistente(pedidoItemAtualizado);

            var item = _pedidoItems.First(x => x.ProdutoId == pedidoItemAtualizado.ProdutoId);

            _pedidoItems.Remove(item);
            _pedidoItems.Add(pedidoItemAtualizado);

            CalcularValorTotal();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemExistente(pedidoItem);

            _pedidoItems.Remove(pedidoItem);

            CalcularValorTotal();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplivavel();

            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalcularValorTotal();

            return result;
        }
    }
}
