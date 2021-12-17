using NerdStore.Vendas.Core;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicinarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto test", 2, 100);


            //Act
            pedido.AdicionarItem(pedidoItem);

            //Assert

            Assert.Equal(200, pedido.ValorTotal);

        }
        [Fact(DisplayName = "ADicoinar Item Pedido Existente")]
        [Trait("CAtegoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_PEdidoExistente_DeveIncrementarUnidadesSomarValores()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(newProdutoId, "Produto test", 2, 100);
            var pedidoItem2 = new PedidoItem(newProdutoId, "Produto test", 1, 100);


            //Act
            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);

            //Assert

            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItems.Count);
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(x => x.ProdutoId == newProdutoId)?.Quantidade);
        }
        [Fact(DisplayName = "ADicoinar Item Pedido Existente")]
        [Trait("CAtegoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_PEdidoExistenteAcimaDaQuantidadeMaxima_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(newProdutoId, "Produto test", 1, 100);
            var pedidoItem2 = new PedidoItem(newProdutoId, "Produto test", Pedido.MAX_UNIDADES_ITEM_PEDIDO, 100);


            //Act
            pedido.AdicionarItem(pedidoItem);


            //Assert
            Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));

        }
        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItemAtualizado = new PedidoItem(newProdutoId, "Produto test", 1, 100);


            //Act & Assert
            Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));

        }
        [Fact(DisplayName = "Atualizar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemValido_DeveAtualizarQuantidade()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(newProdutoId, "Produto test", 2, 100);

            var pedidoItemAtualizado = new PedidoItem(newProdutoId, "Produto test", 5, 100);

            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            pedido.AdicionarItem(pedidoItem);

            //Act 
            pedido.AtualizarItem(pedidoItemAtualizado);

            //Assert
            Assert.Equal(novaQuantidade, pedido.PedidoItems.First(x => x.ProdutoId == newProdutoId).Quantidade);

        }
        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto test 1", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(newProdutoId, "Produto test 2", 2, 100);

            pedido.AdicionarItem(pedidoItemExistente1);
            pedido.AdicionarItem(pedidoItemExistente2);


            var pedidoItem1Atualizado = new PedidoItem(newProdutoId, "Produto test", 5, 100);

            var valorTotal = (pedidoItemExistente1.ValorUnitario * pedidoItemExistente1.Quantidade) +
                             (pedidoItem1Atualizado.ValorUnitario * pedidoItem1Atualizado.Quantidade);

            //Act 
            pedido.AtualizarItem(pedidoItem1Atualizado);

            //Assert
            Assert.Equal(valorTotal, pedido.ValorTotal);

        }
        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(newProdutoId, "Produto test 2", 2, 100);


            //Act & Assert

            Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItem));
        }
        [Fact(DisplayName = "Remover Item Pedido Deve Diminuir quantidade de pedidos")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemValido_DeveDiminuirQuantidadeDePedidoItem()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(newProdutoId, "Produto test", 2, 100);
            var pedidoItem2 = new PedidoItem(newProdutoId, "Produto test", 2, 100);

            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);


            var quantidadePedidoItems = pedido.PedidoItems.Count - 1;

            //Act 
            pedido.RemoverItem(pedidoItem);

            //Assert
            Assert.Equal(quantidadePedidoItems, pedido.PedidoItems.Count);

        }
        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemValido_DeveAtualizarValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var newProdutoId = Guid.NewGuid();

            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto test", 2, 100);
            var pedidoItem2 = new PedidoItem(newProdutoId, "Produto test", 2, 100);

            var pedidoItem2Remover = new PedidoItem(newProdutoId, "Produto test", 2, 100);

            pedido.AdicionarItem(pedidoItem);
            pedido.AdicionarItem(pedidoItem2);

            var novoValorTotal = pedido.ValorTotal - pedidoItem.CalcularValor();


            //Act 
            pedido.RemoverItem(pedidoItem2Remover);

            //Assert
            Assert.Equal(novoValorTotal, pedido.ValorTotal);

        }
        [Fact(DisplayName = "Aplicar voucher valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErrors()
        {
            //Arrange
            var voucher = new Voucher("Teste", 10, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            //Act 

            var result = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
        }
        [Fact(DisplayName = "Aplicar voucher valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInValido_DeveRetornarComErrors()
        {
            //Arrange
            var voucher = new Voucher("", null,
                                           null, 0,
                                           TipoDescontoVoucher.Percentual, DateTime.Now.AddDays(-1),
                                           false, true);

            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            //Act 

            var result = pedido.AplicarVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);
        }
        [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Teste 1", 1, 10);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Teste 2", 2, 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("Teste", 10, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var valorDesconto = pedido.ValorTotal - voucher.ValorDesconto;

            //Act
            
            pedido.AplicarVoucher(voucher);

            //Assert

            Assert.Equal(valorDesconto, pedido.ValorTotal);

        }
        [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarPercentualDoValorTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Teste 1", 1, 10);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Teste 2", 2, 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("Teste", null, 15, 1, TipoDescontoVoucher.Percentual, DateTime.Now.AddDays(10), true, false);

            var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorTotalDesconto = pedido.ValorTotal - valorDesconto;

            //Act

            pedido.AplicarVoucher(voucher);

            //Assert

            Assert.Equal(valorTotalDesconto, pedido.ValorTotal);

        }
        [Fact(DisplayName = "Aplicar voucher desconto exece valor total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPEdido_PedidoDeveTerValorZero()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Teste 1", 1, 10);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Teste 2", 2, 15);

            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("Teste", 1000, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            //Act

            pedido.AplicarVoucher(voucher);

            //Assert

            Assert.Equal(0, pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularValorDescontoTotal()
        {
            //Arrange
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Teste 1", 1, 10);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Teste 2", 2, 15);

            var pedidoItem3 = new PedidoItem(Guid.NewGuid(), "Teste 2", 2, 15);

            var voucher = new Voucher("Teste", 15, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

            var valorEsperado = (pedidoItem1.CalcularValor() + pedidoItem2.CalcularValor() + pedidoItem3.CalcularValor()) - voucher.ValorDesconto;


            pedido.AdicionarItem(pedidoItem1);
            pedido.AdicionarItem(pedidoItem2);
            pedido.AplicarVoucher(voucher);

            //Act

            pedido.AdicionarItem(pedidoItem3);


            //Assert

            Assert.Equal(valorEsperado, pedido.ValorTotal);
        }
    }
}