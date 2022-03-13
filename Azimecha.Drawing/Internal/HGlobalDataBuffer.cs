using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public class HGlobalDataBuffer<T> : IDataBuffer where T : unmanaged {
        private SafeGlobalHandle _hData = new SafeGlobalHandle();
        private int _nCount = 0;

        public unsafe HGlobalDataBuffer(int nObjectCount) {
            _hData.TakeObject(Marshal.AllocHGlobal(nObjectCount * sizeof(T)), true);
            _nCount = nObjectCount;
        }

        public IntPtr DataPointer => _hData.Handle;
        public unsafe long DataSize => _nCount * sizeof(T);

        public unsafe T this[int nIndex] {
            get {
                if (nIndex > _nCount)
                    throw new IndexOutOfRangeException();
                return ((T*)DataPointer)[nIndex];
            }

            set {
                if (nIndex > _nCount)
                    throw new IndexOutOfRangeException();
                ((T*)DataPointer)[nIndex] = value;
            }
        }

        public void Dispose() {
            _hData?.Dispose();
        }

        public static unsafe HGlobalDataBuffer<T> FromStream(System.IO.Stream stmData) {
            long nLength = stmData.Length;

            if (nLength == 0)
                throw new FormatException("Stream is empty.");
            else if (nLength < 0)
                throw new FormatException("Stream length cannot be determined.");

            long nPosition = stmData.Position;

            if (nPosition < 0)
                throw new FormatException("Stream position cannot be determined.");

            long nItems = (nLength - nPosition) / sizeof(T);

            if (nItems > int.MaxValue)
                throw new FormatException("Stream is too long (over 2 billion items).");

            HGlobalDataBuffer<T> buf = new HGlobalDataBuffer<T>((int)nItems);
            Utils.ReadStreamToPointer(stmData, buf.DataPointer, buf.DataSize);
            return buf;
        }
    }

    public class SafeGlobalHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Marshal.FreeHGlobal(hObject);
        }
    }
}
