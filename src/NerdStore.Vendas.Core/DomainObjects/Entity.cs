using NerdStore.Vendas.Core.Messages;

namespace NerdStore.Vendas.Core.DomainObjects
{
    public class Entity
    {
        public Guid Id { get; private set; }

        private List<Event> _notificacoes;

        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();

            _notificacoes.Add(evento);
        }
        public void RemoverEvento(Event evento)
        {
            _notificacoes?.Remove(evento);
        }

        public void LimparEventos()
        {
            _notificacoes = new List<Event>();
        }
    }
}
