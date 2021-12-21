using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly PedidoCommandHandler _handler;

        public PedidoCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<PedidoCommandHandler>();
        }

        [Fact(DisplayName = "Adicionar Item novo Pedido com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            //Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
            //Act

            var result = await _handler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.Adicionar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);

        }
        [Fact(DisplayName = "Adicionar  novo Item Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(clienteId);

            var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);

            pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId, Guid.NewGuid(), "Produto Teste", 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(clienteId)).Returns(Task.FromResult(pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));
            
            //Act

            var result = await _handler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);

        }
        [Fact(DisplayName = "Adicionar Item novo Item Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
        {
            //Arrange
            var clienteId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(clienteId);

            var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);

            pedido.AdicionarItem(pedidoItemExistente);

            var pedidoCommand = new AdicionarItemPedidoCommand(clienteId, pedidoItemExistente.ProdutoId, pedidoItemExistente.ProdutoNome, 2, 100);

            _mocker.GetMock<IPedidoRepository>().Setup(x => x.ObterPedidoRascunhoPorClienteId(clienteId)).Returns(Task.FromResult(pedido));
            _mocker.GetMock<IPedidoRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act

            var result = await _handler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.True(result);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.Atualizar(It.IsAny<Pedido>()), Times.Once);
            _mocker.GetMock<IPedidoRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);

        }
        [Fact(DisplayName = "Adicionar Item novo Item Pedido Rascunho com sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async void AdicionarItem_ComandoInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
        {
            //Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

            //Act

            var result = await _handler.Handle(pedidoCommand, CancellationToken.None);


            //Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));

        }
    }
}
