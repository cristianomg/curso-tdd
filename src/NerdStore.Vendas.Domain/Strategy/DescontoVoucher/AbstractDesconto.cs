using NerdStore.Vendas.Core;

namespace NerdStore.Vendas.Domain.Strategy.DescontoVoucher
{
    public abstract class AbstractDesconto
    {
        public abstract decimal CalcularDesconto(decimal valorTotal, Voucher voucher);

        public static AbstractDesconto Factory (TipoDescontoVoucher tipoDesconto)
        {
            switch (tipoDesconto)
            {
                case TipoDescontoVoucher.Valor:
                    return new DescontoValor();
                case TipoDescontoVoucher.Percentual:
                    return new DescontoPercentual();
                default:
                    throw new DomainException("Invalid Operation");
            }
        }
    }
}
