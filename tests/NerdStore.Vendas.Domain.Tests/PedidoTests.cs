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
            Assert.Equal(3, pedido.PedidoItems.FirstOrDefault(x=>x.ProdutoId == newProdutoId)?.Quantidade);
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

    }
}
