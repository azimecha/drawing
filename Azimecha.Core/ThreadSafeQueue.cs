using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Core {
    public class ThreadSafeQueue<T> : IDisposable {
        private Queue<T> _queueInternal = new Queue<T>();
        private object _objMutex = new object();
        private Semaphore _semAvailable = new Semaphore(0, int.MaxValue);

        protected virtual bool PerformWait(WaitHandle wh) => wh.WaitOne();
        protected virtual bool PerformWait(WaitHandle wh, int nTimeoutMillis) => wh.WaitOne(nTimeoutMillis);
        protected virtual bool PerformWait(WaitHandle wh, TimeSpan tsTimeout) => wh.WaitOne(tsTimeout);

        public void Enqueue(T val) {
            lock (_objMutex) {
                _queueInternal.Enqueue(val);
                _semAvailable.Release();
            }
        }

        public T Dequeue() {
            if (!PerformWait(_semAvailable))
                throw new TimeoutException();

            return DequeueWithoutWait();
        }

        public T Dequeue(int nTimeoutMillis) {
            if (!PerformWait(_semAvailable, nTimeoutMillis))
                throw new TimeoutException();

            return DequeueWithoutWait();
        }

        public T Dequeue(TimeSpan tsTimeout) {
            if (!PerformWait(_semAvailable, tsTimeout))
                throw new TimeoutException();

            return DequeueWithoutWait();
        }

        public bool TryDequeue(out T val)
            => TryDequeue(out val, 0);

        public bool TryDequeue(out T val, int nTimeoutMillis) {
            if (!PerformWait(_semAvailable, nTimeoutMillis)) {
                val = default(T);
                return false;
            }

            val = DequeueWithoutWait();
            return true;
        }

        public bool TryDequeue(out T val, TimeSpan tsTimeout) {
            if (!PerformWait(_semAvailable, tsTimeout)) {
                val = default(T);
                return false;
            }

            val = DequeueWithoutWait();
            return true;
        }

        private T DequeueWithoutWait() {
            lock (_objMutex)
                return _queueInternal.Dequeue();
        }

        public void Dispose() {
            _queueInternal = null;
            _objMutex = null;

            _semAvailable.Release(int.MaxValue);
            _semAvailable = null;
        }
    }
}
