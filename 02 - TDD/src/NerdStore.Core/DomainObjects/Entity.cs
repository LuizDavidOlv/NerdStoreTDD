using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        private List<Event> PrivateNotifications;
        public IReadOnlyCollection<Event> PublicNotifications => PrivateNotifications.AsReadOnly();


        public Entity()
        {
            Id = Guid.NewGuid();
        }


        public void AddEvent(Event eventName)
        {
            PrivateNotifications ??= new List<Event>();
            PrivateNotifications.Add(eventName);
        }

        public void RemoveEvent(Event eventName)
        {
            PrivateNotifications?.Remove(eventName);
        }

        public void ClearEvents()
        {
            PrivateNotifications?.Clear();
        }


    }
}