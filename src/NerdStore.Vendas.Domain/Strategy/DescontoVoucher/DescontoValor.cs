namespace NerdStore.Vendas.Domain.Strategy.DescontoVoucher
{
    public class DescontoValor : AbstractDesconto
    {
        public override decimal CalcularDesconto(decimal valorTotal, Voucher voucher)
        {
            decimal desconto = 0;
            if (voucher.ValorDesconto.HasValue)
                desconto = voucher.ValorDesconto.Value;

            return desconto;
        }
    }
}
