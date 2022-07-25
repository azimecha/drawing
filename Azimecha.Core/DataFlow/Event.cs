using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core.DataFlow {
    public interface IEvent { }
    public interface IEvent<TDelegate> : IEvent where TDelegate : Delegate { }

    public abstract class Event : IEvent {
        private Dictionary<Weak<EventHandler>, EventHandler> _dicHandlers = new Dictionary<Weak<EventHandler>, EventHandler>();

        internal void AddHandler(EventHandler handler, bool bStrong) {
            lock (_dicHandlers)
                _dicHandlers.Add(new Weak<EventHandler>(handler), bStrong ? handler : null);
        }

        internal void RemoveHandler(EventHandler handler) {
            lock (_dicHandlers)
                _dicHandlers.Remove(new Weak<EventHandler>(handler));
        }

        public void Trigger(params object[] arrArgs) {
            List<EventHandler> lstToTrigger = new List<EventHandler>();

            lock (_dicHandlers) {
                List<Weak<EventHandler>> lstToRemove = new List<Weak<EventHandler>>();

                foreach (Weak<EventHandler> refHandler in _dicHandlers.Keys) {
                    EventHandler handler = refHandler.Target;
                    if (handler is null)
                        lstToRemove.Add(refHandler);
                    else
                        lstToTrigger.Add(handler);
                }

                foreach (Weak<EventHandler> refDeletedHandler in lstToRemove)
                    _dicHandlers.Remove(refDeletedHandler);
            }

            foreach (EventHandler handler in lstToTrigger)
                handler.Trigger(arrArgs);
        }
    }

    public class Event<TDelegate> : Event, IEvent<TDelegate> where TDelegate : Delegate { }

    public class EventOf<T> : Event<Action<T>> {
        public void Trigger(T obj) => Trigger((object)obj);
    }
}
