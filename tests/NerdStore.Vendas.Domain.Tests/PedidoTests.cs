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
    }
}