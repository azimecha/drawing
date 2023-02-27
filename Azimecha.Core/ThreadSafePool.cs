using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Core {
    public class ThreadSafePool<T> : IDisposable {
        private ThreadSafeQueue<T> _queueExisting = new ThreadSafeQueue<T>();
        private Factories<T>.FactoryDelegate _procDefaultFactory;

        public ThreadSafePool() {
            _procDefaultFactory = Factories<T>.DefaultFactory;
        }

        public ThreadSafePool(Factories<T>.FactoryDelegate procFactory) {
            _procDefaultFactory = procFactory;
        }

        public ThreadSafePool(params object[] arrArgs) {
            _procDefaultFactory = () => Factories<T>.ParameterizedFactory(arrArgs);
        }

        public T TakeItem() => TakeItem(_procDefaultFactory);

        public T TakeItem(Factories<T>.FactoryDelegate procIfNoneAvailable) {
            if (_queueExisting.TryDequeue(out T valExisting))
                return valExisting;

            return procIfNoneAvailable();
        }

        public void ReturnItem(T valToReturn) {
            _queueExisting.Enqueue(valToReturn);
        }

        public void Dispose() {
            Interlocked.Exchange(ref _queueExisting, null)?.Dispose();
            _procDefaultFactory = null;
        }
    }
}
