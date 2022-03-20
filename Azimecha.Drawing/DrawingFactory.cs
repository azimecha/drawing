using Azimecha.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Azimecha.Drawing {
    public static class DrawingFactory {
        public static IDrawingAPI GetDrawingAPI()
            => (IDrawingAPI)Activator.CreateInstance(Assembly.Load("Azimecha.Drawing.AGG").GetType("Azimecha.Drawing.AGG.DrawingAPI"));

        public static IDataBuffer CreateBuffer(int nSize)
            => new HGlobalDataBuffer<byte>(nSize);

        public static IDataBuffer CreateBuffer<T>(int nCount) where T : unmanaged
            => new HGlobalDataBuffer<T>(nCount);

        public static IDataBuffer CreateBufferFrom(string strFilePath)
            => MemoryMapping.MapFileReadOnly(strFilePath, true);

        public static IDataBuffer CreateBufferFrom<T>(T[] arrData) where T : unmanaged {
            using (PinnedArrayDataBuffer<T> bufTemp = new PinnedArrayDataBuffer<T>(arrData))
                return HGlobalDataBuffer<T>.FromExisting(bufTemp);
        }

        public static IDataBuffer CreateBufferFrom(System.IO.Stream stmData)
            => HGlobalDataBuffer<byte>.FromStream(stmData);

        public static IDataBuffer CreateBufferFrom<T>(System.IO.Stream stmData) where T : unmanaged
            => HGlobalDataBuffer<T>.FromStream(stmData);

        public static IDataBuffer CreateBufferFrom(IntPtr pData, int nSize) {
            IDataBuffer buf = new HGlobalDataBuffer<byte>(nSize);
            Utils.CopyMemory(pData, buf.DataPointer, (ulong)nSize);
            return buf;
        }

        public static IDataBuffer CreateBufferOn<T>(T[] arrData) where T : unmanaged
            => new PinnedArrayDataBuffer<T>(arrData);
    }
}
