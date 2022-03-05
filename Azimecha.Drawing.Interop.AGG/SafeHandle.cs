using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Azimecha.Drawing.AGG {
    internal abstract class SafeHandle : IDisposable {
        private IntPtr _hObject;
        private bool _bOwnsObject;

        public SafeHandle(IntPtr hObject, bool bOwnObject) {
            _hObject = hObject;
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
    }
}
