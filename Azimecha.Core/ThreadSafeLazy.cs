using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public class ThreadSafeLazy<T> {
        private T _objValue;
        private FactoryDelegate _procFactory;
        private object[] _arrParams;
        private object _objMutex = new object();
        private bool _bInitComplete = false;

        public delegate T FactoryDelegate();

        public ThreadSafeLazy() {
            _procFactory = DefaultFactory;
        }

        public ThreadSafeLazy(FactoryDelegate procFactory) {
            _procFactory = procFactory;
        }

        public ThreadSafeLazy(params object[] arrCtorParams) {
            _arrParams = arrCtorParams;
            _procFactory = ParameterizedFactory;
        }

        private T DefaultFactory()
            => (T)Activator.CreateInstance(typeof(T), nonPublic: true);

        private T ParameterizedFactory()
            => (T)Activator.CreateInstance(typeof(T), bindingAttr: System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance, binder: null, args: _arrParams, culture: null);

        public T Value {
            get {
                if (!_bInitComplete)
                    lock (_objMutex)
                        if (!_bInitComplete)
                            Init();

                return _objValue;
            }
        }

        private void Init() {
            _objValue = _procFactory();
            _procFactory = null;
            _arrParams = null;
            _bInitComplete = true;
        }
    }
}
