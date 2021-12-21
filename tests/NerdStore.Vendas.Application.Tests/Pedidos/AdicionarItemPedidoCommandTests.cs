using NerdStore.Vendas.Application.Commands;
using System;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item command valido")]
        [Trait("Categoria", "Vendas - Pedido Comands")]
        public void AdicionarItemPedidoCommand_ComandoEstaValido_DEvePassarNaValidacao()
        {
            //Arrange
            var pedidoComamnd = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            //Act

            var result = pedidoComamnd.EhValido();


            //Assert
            Assert.True(result);

        }
        [Fact(DisplayName = "Adicionar Item command Invalido")]
        [Trait("Categoria", "Vendas - Pedido Comands")]
        public void AdicionarItemPedidoCommand_ComandoEstaInValido_NaoDEvePassarNaValidacao()
        {
            //Arrange
            var pedidoComamnd = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            //Act

            var result = pedidoComamnd.EhValido();


            //Assert
            Assert.False(result);
            Assert.Equal(5, pedidoComamnd.ValidationResult.Errors.Count);

        }
    }

}
