using FluentValidation;
using NerdStore.Vendas.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }
    public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public AdicionarItemPedidoValidation()
        {
            RuleFor(x => x.ClienteId)
                .NotEqual(Guid.Empty);

            RuleFor(x=>x.ProdutoId)
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Nome)
                .NotEmpty();

            RuleFor(x => x.Quantidade)
                .GreaterThan(Pedido.MIN_UNIDADES_ITEM_PEDIDO)
                .LessThanOrEqualTo(Pedido.MAX_UNIDADES_ITEM_PEDIDO);

            RuleFor(x => x.ValorUnitario)
                .GreaterThan(0);
        }
    }

}
