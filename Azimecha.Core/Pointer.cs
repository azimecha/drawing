using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public unsafe struct Pointer<T> where T : unmanaged {
        private T* _pValue;

        public Pointer(IntPtr pValue) {
            _pValue = (T*)pValue;
        }

        public Pointer(T* pValue) {
            _pValue = pValue;
        }

        public T ValueAtTarget {
            get => *_pValue;
            set => *_pValue = value;
        }
    }
}
