using NerdStore.Vendas.Core;
using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 15,
                                      null, 1,
                                      TipoDescontoVoucher.Valor,DateTime.Now.AddDays(15),
                                      true, false);
            //Act

            var result = voucher.ValidarSeAplivavel();

            //Assert

            Assert.True(result.IsValid);
        }
        [Fact(DisplayName = "Validar Voucher Tipo Valor InValido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInValido()
        {
            //Arrange
            var voucher = new Voucher("", null,
                                      null, 0,
                                      TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1),
                                      false, true);
            //Act

            var result = voucher.ValidarSeAplivavel();

            //Assert

            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
        }
        [Fact(DisplayName = "Validar Voucher Tipo Percentual Valido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPercentual_DeveEstarValido()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-REAIS", null,
                                      10, 1,
                                      TipoDescontoVoucher.Percentual, DateTime.Now.AddDays(15),
                                      true, false);
            //Act

            var result = voucher.ValidarSeAplivavel();

            //Assert

            Assert.True(result.IsValid);
        }
        [Fact(DisplayName = "Validar Voucher Tipo Percentual InValido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoPercentual_DeveEstarInValido()
        {
            //Arrange
            var voucher = new Voucher("", null,
                                      null, 0,
                                      TipoDescontoVoucher.Percentual, DateTime.Now.AddDays(-1),
                                      false, true);
            //Act

            var result = voucher.ValidarSeAplivavel();

            //Assert

            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
        }

    }
}
