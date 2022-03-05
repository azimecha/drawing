using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Drawing.AGG {
    internal abstract class SafeHandle : IDisposable {
        private IntPtr _hObject;
        private bool _bOwnsObject;

        public SafeHandle() {
            _hObject = GetInvalidHandleValue();
            _bOwnsObject = false;
        }

        public SafeHandle(IntPtr hObject, bool bOwnObject) {
            _hObject = hObject;
            _bOwnsObject = bOwnObject;
        }

        public void TakeObject(IntPtr hObject, bool bOwnObject) {
            if (Interlocked.CompareExchange(ref _hObject, hObject, GetInvalidHandleValue()) != GetInvalidHandleValue())
                throw new InvalidOperationException("The SafeHandle already contains a value");
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

        protected void Dispose(bool bDisposing) {
            IntPtr hObject = Interlocked.Exchange(ref _hObject, GetInvalidHandleValue());
            if (_bOwnsObject && CheckHandleValid(hObject))
                CloseObjectHandle(hObject);
            _bOwnsObject = false;
        }

        ~SafeHandle() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(bDisposing: false);
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(bDisposing: true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
            => "{" + GetType().Name + ":" + _hObject.ToInt64().ToString("X" + IntPtr.Size * 2) + "}";

        public override bool Equals(object obj)
            => (obj is SafeHandle h) && (_hObject == h._hObject);

        public override int GetHashCode()
            => _hObject.GetHashCode();
    }
}
