using MediatR;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Core.DomainObjects;
using NerdStore.Vendas.Core.Messages;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;
        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }
        public async Task<bool> Handle(AdicionarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!await ValidarComando(request, cancellationToken))
                return false;

            var pedidoItem = new PedidoItem(request.ProdutoId, request.Nome, request.Quantidade, request.ValorUnitario);

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido is null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(request.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(pedido);

            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);

                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.First(x => x.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }

                _pedidoRepository.Atualizar(pedido);
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(request.ClienteId, pedido.Id, request.ProdutoId,
                                                                              request.Nome, request.ValorUnitario,
                                                                              request.Quantidade));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> ValidarComando(Command command, CancellationToken cancellationToken )
        {
            if (command.EhValido())
                return true;

            foreach (var error in command.ValidationResult.Errors)
                await _mediator.Publish(new DomainNotification(command.MessageType, error.ErrorMessage), cancellationToken);
            
            return false;
        }
    }
}
