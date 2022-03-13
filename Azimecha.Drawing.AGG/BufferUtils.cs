using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG {

    internal static class BufferUtils {
        public static readonly Interop.AwDataDestructor DisposingDestructor = DataDisposeDtorProc;
        public static readonly Interop.AwDataDestructor NondisposingDestructor = DataNoDisposeDtorProc;

        private static void DataDisposeDtorProc(IntPtr pDataIgnored, IntPtr hGCHandle) {
            GCHandle gchBufferObject = GCHandle.FromIntPtr(hGCHandle);
            (gchBufferObject.Target as IDisposable)?.Dispose();
            gchBufferObject.Free();
        }

        private static void DataNoDisposeDtorProc(IntPtr pDataIgnored, IntPtr hGCHandle) {
            GCHandle gchBufferObject = GCHandle.FromIntPtr(hGCHandle);
            gchBufferObject.Free();
        }

        public static void TryPassOwnership(IDataBuffer buf, bool bDisposeInDtorCallback, Action<Interop.AwBufferInfo> procTry) {
            GCHandle? gchBufferObject = GCHandle.Alloc(buf);
            try {
                Interop.AwBufferInfo infBuffer = new Interop.AwBufferInfo() {
                    nDataSize = (ulong)buf.DataSize,
                    pData = buf.DataPointer,
                    procDestructor = bDisposeInDtorCallback ? DisposingDestructor : NondisposingDestructor,
                    tag = GCHandle.ToIntPtr(gchBufferObject.Value)
                };

                procTry(infBuffer);
                gchBufferObject = null;
            } finally {
                gchBufferObject?.Free();
            }
        }
    }
}
