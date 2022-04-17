using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Core {
    public abstract class OrderedSafeHandle : OrderedDisposable, IHandle {
        private IntPtr _hObject;
        private bool _bOwnsObject;

        public OrderedSafeHandle() {
            _hObject = GetInvalidHandleValue();
            _bOwnsObject = false;
        }

        public OrderedSafeHandle(IntPtr hObject, bool bOwnObject) {
            _hObject = hObject;
            _bOwnsObject = bOwnObject;
        }

        public void TakeObject(IntPtr hObject, bool bOwnObject) {
            if (Disposed)
                throw new InvalidOperationException("An OrderedSafeHandle cannot be reinitialized after being disposed");

            if (Interlocked.CompareExchange(ref _hObject, hObject, GetInvalidHandleValue()) != GetInvalidHandleValue())
                throw new InvalidOperationException("The OrderedSafeHandle already contains a value");

            _bOwnsObject = bOwnObject;
        }

        public IntPtr UncheckedHandle => _hObject;

        public IntPtr Handle {
            get {
                IntPtr hObject = _hObject;
                if (!CheckHandleValid(hObject))
                    throw new InvalidOperationException($"The handle is not valid");
                return hObject;
            }
        }

        public bool IsHandleValid => CheckHandleValid(_hObject);

        protected abstract void CloseObjectHandle(IntPtr hObject);
        protected virtual bool CheckHandleValid(IntPtr hObject) => hObject != GetInvalidHandleValue();
        protected virtual IntPtr GetInvalidHandleValue() => IntPtr.Zero;

        protected override void Dispose(bool bDisposing) {
            // Do not change this code. Override CloseObjectHandle.
            IntPtr hObject = Interlocked.Exchange(ref _hObject, GetInvalidHandleValue());
            if (_bOwnsObject && CheckHandleValid(hObject))
                CloseObjectHandle(hObject);
            _bOwnsObject = false;
        }

        public override string ToString()
            => "{" + GetType().Name + ":" + _hObject.ToInt64().ToString("X" + IntPtr.Size * 2) + "}";

        public override bool Equals(object obj)
            => obj is IHandle h && _hObject == h.UncheckedHandle;

        public override int GetHashCode()
            => _hObject.GetHashCode();
    }
}
