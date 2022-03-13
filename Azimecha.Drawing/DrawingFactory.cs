using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Azimecha.Drawing {
    public static class DrawingFactory {
        public static IDrawingAPI GetDrawingAPI()
            => (IDrawingAPI)Activator.CreateInstance(Assembly.Load("Azimecha.Drawing.AGG").GetType("Azimecha.Drawing.AGG.DrawingAPI"));

        public static IDataBuffer CreateBuffer(int nSize)
            => new Internal.HGlobalDataBuffer<byte>(nSize);

        public static IDataBuffer CreateBuffer<T>(int nCount) where T : unmanaged
            => new Internal.HGlobalDataBuffer<T>(nCount);

        public static IDataBuffer CreateBufferFrom(string strFilePath)
            => Internal.MemoryMapping.MapFileReadOnly(strFilePath, true);

        public static IDataBuffer CreateBufferFrom<T>(T[] arrData) where T : unmanaged {
            using (Internal.PinnedArrayDataBuffer<T> bufTemp = new Internal.PinnedArrayDataBuffer<T>(arrData))
                return Internal.HGlobalDataBuffer<T>.FromExisting(bufTemp);
        }

        public static IDataBuffer CreateBufferFrom(System.IO.Stream stmData)
            => Internal.HGlobalDataBuffer<byte>.FromStream(stmData);

        public static IDataBuffer CreateBufferFrom<T>(System.IO.Stream stmData) where T : unmanaged
            => Internal.HGlobalDataBuffer<T>.FromStream(stmData);

        public static IDataBuffer CreateBufferFrom(IntPtr pData, int nSize) {
            IDataBuffer buf = new Internal.HGlobalDataBuffer<byte>(nSize);
            Internal.Utils.CopyMemory(pData, buf.DataPointer, (ulong)nSize);
            return buf;
        }

        public static IDataBuffer CreateBufferOn<T>(T[] arrData) where T : unmanaged
            => new Internal.PinnedArrayDataBuffer<T>(arrData);
    }
}
