using NerdStore.Vendas.Core;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ChecarSeQuantidadeEstaNoRangePermitido(quantidade);

            this.ProdutoId = produtoId;
            this.ProdutoNome = produtoNome;
            this.Quantidade = quantidade;
            this.ValorUnitario = valorUnitario;
        }
        private void ChecarSeQuantidadeEstaNoRangePermitido(int quantidade)
        {
            if (quantidade > Pedido.MAX_UNIDADES_ITEM_PEDIDO)
                throw new DomainException($"Quantidade não deve ser menor que {Pedido.MAX_UNIDADES_ITEM_PEDIDO}");
            if (quantidade < Pedido.MIN_UNIDADES_ITEM_PEDIDO)
                throw new DomainException($"Quantidade não deve ser maior que {Pedido.MIN_UNIDADES_ITEM_PEDIDO}");
        }
        public void AdicionarUnidades(int unidades)
        {
            ChecarSeQuantidadeEstaNoRangePermitido(Quantidade + unidades);

            Quantidade += unidades;
        }
        public decimal CalcularValor()
        {
            return ValorUnitario * Quantidade;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            return this.ProdutoId == ((PedidoItem)obj).ProdutoId;
        }

        public override int GetHashCode()
        {
            return 10 ^ this.ProdutoId.GetHashCode();
        }
    }
}
