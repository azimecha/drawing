using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public class ThreadSafeLazy<T> {
        private T _objValue;
        private Factories<T>.FactoryDelegate _procFactory;
        private object _objMutex = new object();
        private bool _bInitComplete = false;

        public ThreadSafeLazy() {
            _procFactory = Factories<T>.DefaultFactory;
        }

        public ThreadSafeLazy(Factories<T>.FactoryDelegate procFactory) {
            _procFactory = procFactory;
        }

        public ThreadSafeLazy(params object[] arrCtorParams) {
            _procFactory = () => Factories<T>.ParameterizedFactory(arrCtorParams);
        }

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
            _bInitComplete = true;
        }
    }
}
