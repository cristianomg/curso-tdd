using MediatR;
using NerdStore.Vendas.Core.Messages;

namespace NerdStore.Vendas.Core.DomainObjects
{
    public class DomainNotification : Message, INotification
    {
        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
            Timestamp = DateTime.Now;
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
        }

        public DateTime Timestamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }


    }
}
