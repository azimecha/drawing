using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Azimecha.Drawing.AGG {
    public interface IDataBuffer : IDisposable {
        IntPtr DataPointer { get; }
        long DataSize { get; }
    }

    internal class PinnedArrayDataBuffer<T> : IDataBuffer {
        private SafePinnedGCHandle _hObject;
        private T[] _arrData;

        public PinnedArrayDataBuffer(T[] arrData) {
            _arrData = arrData;
            _hObject = new SafePinnedGCHandle(arrData);
        }

        public IntPtr DataPointer => _hObject.PinnedAddress;
        public long DataSize => _arrData.Length * Marshal.SizeOf(typeof(T));

        public void Dispose() {
            Interlocked.Exchange(ref _hObject, null)?.Dispose();
            _arrData = null;
        }
    }

    internal class SafePinnedGCHandle : Internal.SafeHandle {
        public SafePinnedGCHandle(object obj) : base(GCHandle.ToIntPtr(GCHandle.Alloc(obj, GCHandleType.Pinned)), true) { }
        public SafePinnedGCHandle(IntPtr hObject, bool bOwnObject) : base(hObject, bOwnObject) { }

        protected override void CloseObjectHandle(IntPtr hObject) {
            GCHandle.FromIntPtr(hObject).Free();
        }

        public object PinnedObject => GCHandle.FromIntPtr(Handle).Target;
        public IntPtr PinnedAddress => GCHandle.FromIntPtr(Handle).AddrOfPinnedObject();
    }
}
