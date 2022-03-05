using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Drawing.AGG {
    internal abstract class SafeHandle : IDisposable {
        private IntPtr _hObject;
        private bool _bOwnsObject;

        public SafeHandle() {
            _hObject = IntPtr.Zero;
            _bOwnsObject = false;
        }

        public SafeHandle(IntPtr hObject, bool bOwnObject) {
            _hObject = hObject;
            _bOwnsObject = bOwnObject;
        }

        public void TakeObject(IntPtr hObject, bool bOwnObject) {
            if (Interlocked.CompareExchange(ref _hObject, hObject, IntPtr.Zero) != IntPtr.Zero)
                throw new InvalidOperationException("The SafeHandle already contains a value");
            _bOwnsObject = bOwnObject;
        }

        public IntPtr Handle => _hObject;

        protected abstract void CloseObjectHandle(IntPtr hObject);

        protected void Dispose(bool bDisposing) {
            IntPtr hObject = Interlocked.Exchange(ref _hObject, IntPtr.Zero);
            if (_bOwnsObject && (hObject != IntPtr.Zero))
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
            => "{" + GetType().Name + ":" + Handle.ToInt64().ToString("X" + IntPtr.Size * 2) + "}";

        public override bool Equals(object obj)
            => (obj is SafeHandle h) && (_hObject == h._hObject);

        public override int GetHashCode()
            => Handle.GetHashCode();
    }
}
