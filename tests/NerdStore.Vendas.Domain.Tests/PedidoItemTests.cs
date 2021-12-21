using NerdStore.Vendas.Core;
using NerdStore.Vendas.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoItemTests
    {
        [Fact(DisplayName = "Novo item pedido acima do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        { 
            //Arrange & Act && Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto test", Pedido.MAX_UNIDADES_ITEM_PEDIDO + 1, 100));
        }
        [Fact(DisplayName = "Novo item pedido abaixo do permitido")]
        [Trait("Categoria", "Vendas - Pedido Item")]
        public void AdicionarItemPedido_UnidadesItemAbaixoDoPermitido_DeveRetornarException()
        {

            //Arrange & Act && Assert
            Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto test", Pedido.MIN_UNIDADES_ITEM_PEDIDO - 1, 100));
        }
    }
}
