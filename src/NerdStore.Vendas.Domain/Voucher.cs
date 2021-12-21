using FluentValidation;
using FluentValidation.Results;
using NerdStore.Vendas.Core;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public Voucher(string codigo, decimal? valorDesconto, 
                       decimal? percentualDesconto, int quantidade,
                       TipoDescontoVoucher tipoDescontoVoucher, DateTime validade,
                       bool ativo, bool utilizado)
        {
            this.Codigo = codigo;
            this.ValorDesconto = valorDesconto;
            this.PercentualDesconto = percentualDesconto;
            this.Quantidade = quantidade;
            this.TipoDescontoVoucher = tipoDescontoVoucher;
            this.DataValidade = validade;
            this.Ativo = ativo;
            this.Utilizado = utilizado;
        }
        public string Codigo { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }


        public ValidationResult ValidarSeAplivavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public VoucherAplicavelValidation()
        {
            RuleFor(x => x.Ativo).Equal(true);
            RuleFor(x => x.Utilizado).Equal(false);
            RuleFor(x => x.DataValidade).GreaterThan(DateTime.Now);
            RuleFor(x => x.Codigo).NotEmpty();
            RuleFor(x => x.Quantidade).GreaterThan(0);
            When(f => f.TipoDescontoVoucher == TipoDescontoVoucher.Valor, () =>
              {
                  RuleFor(x => x.ValorDesconto).NotNull().GreaterThan(0);

              });
            When(f => f.TipoDescontoVoucher == TipoDescontoVoucher.Percentual, () =>
              {
                  RuleFor(x => x.PercentualDesconto).NotNull().GreaterThan(0);
              });


        }
    }

}
