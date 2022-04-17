using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public abstract class OrderedDisposable : IDisposable {
        private bool _bDisposed;
        private object _objDisposalMutex = new object();

        protected virtual void Dispose(bool bDisposing) { }

        private void DisposeWrapper(bool bDisposing) {
            if (!_bDisposed) {
                lock (_objDisposalMutex) {
                    if (!_bDisposed && CanDispose(this)) {
                        Dispose(bDisposing);
                        _bDisposed = true;
                        RemoveObject(this);
                    }
                }
            }
        }

        ~OrderedDisposable() {
            DisposeWrapper(bDisposing: false);

            if (!_bDisposed) // nope.
                GC.ReRegisterForFinalize(this);
        }

        public void Dispose() {
            DisposeWrapper(bDisposing: true);

            if (_bDisposed)
                GC.SuppressFinalize(this);
        }

        public void DisposeOnlyAfter(OrderedDisposable objDep)
            => AddDependency(this, objDep);

        public void RemoveDisposalRule(OrderedDisposable objDep)
            => RemoveDependency(this, objDep);

        public bool Disposed => _bDisposed;

        // ---- static dependency list ----

        private static List<KeyValuePair<Weak<OrderedDisposable>, Weak<OrderedDisposable>>> _lstDependencies 
            = new List<KeyValuePair<Weak<OrderedDisposable>, Weak<OrderedDisposable>>>();

        private static bool CanDispose(OrderedDisposable obj) {
            lock (_lstDependencies) {
                foreach (KeyValuePair<Weak<OrderedDisposable>, Weak<OrderedDisposable>> kvp in _lstDependencies)
                    if (kvp.Key.Equals(obj) && !(kvp.Value.Target?._bDisposed ?? true))
                        return false;
            }

            return true;
        }

        private static void AddDependency(OrderedDisposable obj, OrderedDisposable objDep) {
            lock (_lstDependencies) {
                _lstDependencies.Add(new KeyValuePair<Weak<OrderedDisposable>, Weak<OrderedDisposable>>
                    (new Weak<OrderedDisposable>(obj), new Weak<OrderedDisposable>(objDep)));
            }
        }

        private static void RemoveDependency(OrderedDisposable obj, OrderedDisposable objDep) {
            Weak<OrderedDisposable> refObj = new Weak<OrderedDisposable>(obj);
            Weak<OrderedDisposable> refDep = new Weak<OrderedDisposable>(objDep);

            lock (_lstDependencies) {
                for (int i = 0; i < _lstDependencies.Count; i++) {
                    if (_lstDependencies[i].Key.Equals(obj) && _lstDependencies[i].Value.Equals(objDep)) {
                        _lstDependencies.RemoveAt(i);
                        return; // remove only one
                    }
                }
            }
        }

        private static void RemoveObject(OrderedDisposable obj) {
            Weak<OrderedDisposable> refObj = new Weak<OrderedDisposable>(obj);

            lock (_lstDependencies) {
                int i = 0;
                while (i < _lstDependencies.Count) {
                    if (_lstDependencies[i].Key.Equals(obj) || _lstDependencies[i].Value.Equals(obj))
                        _lstDependencies.RemoveAt(i);
                    else
                        i++;
                }
            }
        }

        private static void RunDepListGC() { // remove dead objects
            lock (_lstDependencies) {
                int i = 0;
                while (i < _lstDependencies.Count) {
                    if ((_lstDependencies[i].Key.Target?._bDisposed ?? true) || (_lstDependencies[i].Value.Target?._bDisposed ?? true))
                        _lstDependencies.RemoveAt(i);
                    else
                        i++;
                }
            }
        }
    }
}
