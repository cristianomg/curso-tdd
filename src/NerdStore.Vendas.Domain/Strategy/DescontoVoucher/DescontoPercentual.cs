namespace NerdStore.Vendas.Domain.Strategy.DescontoVoucher
{
    public class DescontoPercentual : AbstractDesconto
    {
        public override decimal CalcularDesconto(decimal valorTotal, Voucher voucher)
        {
            decimal desconto = 0;
            if (voucher.PercentualDesconto.HasValue)
            {
                desconto = (valorTotal * voucher.PercentualDesconto.Value) / 100;
            }

            return desconto;
        }
    }
}
