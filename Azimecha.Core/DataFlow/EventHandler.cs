using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Core.DataFlow {
    public abstract class EventHandler : IDisposable {
        private Event _evt = null;

        internal EventHandler(Event evt) {
            _evt = evt;
        }

        internal abstract void Trigger(object[] arrArgs);

        private void DisposeImpl() {
            Event evt = Interlocked.Exchange(ref _evt, null);
            if (evt != null) {
                try {
                    evt.RemoveHandler(this);
                } catch (KeyNotFoundException) { }
            }
        }

        ~EventHandler() {
            Dispose();
        }

        public void Dispose() {
            Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public abstract class EventHandler<TDelegate> : EventHandler where TDelegate : Delegate {
        private TDelegate _procCallback;

        internal EventHandler(Event<TDelegate> evt, TDelegate procCallback) : base(evt) {
            _procCallback = procCallback;
        }

        internal override void Trigger(object[] arrArgs)
            => _procCallback.DynamicInvoke(arrArgs);
    }

    public class WeakEventHandler<TDelegate> : EventHandler<TDelegate> where TDelegate : Delegate {
        public WeakEventHandler(IEvent<TDelegate> evt, TDelegate procCallback) : base((Event<TDelegate>)evt, procCallback) {
            ((Event<TDelegate>)evt).AddHandler(this, false);
        }
    }

    public class StrongEventHandler<TDelegate> : EventHandler<TDelegate> where TDelegate : Delegate {
        public StrongEventHandler(IEvent<TDelegate> evt, TDelegate procCallback) : base((Event<TDelegate>)evt, procCallback) {
            ((Event<TDelegate>)evt).AddHandler(this, true);
        }
    }

    public class WeakEventHandlerOf<T> : WeakEventHandler<Action<T>> {
        public WeakEventHandlerOf(IEvent<Action<T>> evt, Action<T> procCallback) : base(evt, procCallback) { }
    }

    public class StrongEventHandlerOf<T> : StrongEventHandler<Action<T>> {
        public StrongEventHandlerOf(IEvent<Action<T>> evt, Action<T> procCallback) : base(evt, procCallback) { }
    }
}
