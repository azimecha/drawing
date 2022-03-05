using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class Bitmap : IBitmap {
        private SafeBitmapHandle _hBitmap;
        private int _nWidth, _nHeight;

        internal Bitmap(SafeBitmapHandle hBitmap) {
            if (!Interop.Functions.AwGetBitmapSize(hBitmap.Handle, out _nWidth, out _nHeight))
                throw new InfoQueryFailedException($"Unable to get size of bitmap {hBitmap.Handle}");

            _hBitmap = hBitmap;
        }

        public Bitmap(int w, int h) {
            IntPtr hBitmap = Interop.Functions.AwCreateBitmap(w, h);
            if (hBitmap == IntPtr.Zero)
                throw new ObjectCreationFailedException($"Error creating empty {w}x{h} bitmap");

            _hBitmap = new SafeBitmapHandle(hBitmap, true);

            _nWidth = w;
            _nHeight = h;
        }

        public Bitmap(int w, int h, IBitmapDataBuffer buf) {
            int nExpectedLength = w * h * 4;
            if (buf.DataSize < nExpectedLength)
                throw new ArgumentException($"Data buffer too small for {w}x{h} bitmap: expected {nExpectedLength} bytes, got {buf.DataSize}");

            IntPtr hBitmap = Interop.Functions.AwCreateBitmapOnBuffer(w, h, buf.DataPointer, BitmapBufferManager.Destructor);
            if (hBitmap == IntPtr.Zero)
                throw new ObjectCreationFailedException($"Error creating {w}x{h} bitmap");

            _hBitmap = new SafeBitmapHandle(hBitmap, true);
            BitmapBufferManager.Register(buf);

            _nWidth = w;
            _nHeight = h;
        }

        public int Width => _nWidth;
        public int Height => _nHeight;
        public long Stride => _nWidth * 4;
        public long DataSize => Stride * _nHeight;
        public PixelFormat Format => PixelFormat.ARGB32Premul;

        public static Bitmap CreateOnArray(int w, int h, byte[] arrData) {
            int nExpectedLength = w * h * 4;
            if (arrData.Length != nExpectedLength)
                throw new ArgumentException($"Incorrect data size for {w}x{h} bitmap: expected {nExpectedLength} bytes, got {arrData.Length}");
            return new Bitmap(w, h, new PinnedArrayDataBuffer<byte>(arrData));
        }

        public static Bitmap CreateOnArray(int w, int h, uint[] arrData) {
            int nExpectedLength = w * h;
            if (arrData.Length != nExpectedLength)
                throw new ArgumentException($"Incorrect data size for {w}x{h} bitmap: expected {nExpectedLength} pixels, got {arrData.Length}");
            return new Bitmap(w, h, new PinnedArrayDataBuffer<uint>(arrData));
        }

        public Bitmap Duplicate() {
            IntPtr hBitmapUnsafe = Interop.Functions.AwDuplicateBitmap(_hBitmap.Handle);
            if (hBitmapUnsafe == IntPtr.Zero)
                throw new ObjectCreationFailedException($"Error duplicating bitmap {_hBitmap.Handle}");

            SafeBitmapHandle hBitmapSafe = new SafeBitmapHandle(hBitmapUnsafe, true);
            return new Bitmap(hBitmapSafe);
        }

        IBitmap IBitmap.Duplicate() => Duplicate();

        public void Dispose() {
            _hBitmap.Dispose();
        }
    }

    internal class SafeBitmapHandle : SafeHandle {
        public SafeBitmapHandle(IntPtr hObject, bool bOwnObject) : base(hObject, bOwnObject) { }

        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.AwDeleteBitmap(hObject);
        }
    }

    public delegate void DataDestructorProc(IntPtr pData);

    internal static class BitmapBufferManager {
        public static readonly Interop.AwDataDestructor Destructor = DestructorProc;

        private struct BufferInfo {
            public IBitmapDataBuffer Buffer;
            public int RefCount;
        }

        private static Dictionary<IntPtr, BufferInfo> _dicBuffers = new Dictionary<IntPtr, BufferInfo>();
        private static object _objMutex = new object();

        private static void DestructorProc(IntPtr pData) {
            BufferInfo infBuffer;
            bool bDestroy = false;

            lock (_objMutex) {
                bool bFound = _dicBuffers.TryGetValue(pData, out infBuffer);

                if (!bFound) {
                    Debug.WriteLine($"[{nameof(BitmapBufferManager)}] WARNING: Buffer {pData} not found! Not doing anything!");
                    return;
                }

                infBuffer.RefCount--;

                if (infBuffer.RefCount <= 0) {
                    _dicBuffers.Remove(pData);
                    bDestroy = true;
                } else {
                    _dicBuffers[pData] = infBuffer;
                }
            }

            if (bDestroy)
                infBuffer.Buffer.Dispose();
        }

        public static void Register(IBitmapDataBuffer buf) {
            lock (_objMutex) {
                BufferInfo infBuffer;
                bool bFound = _dicBuffers.TryGetValue(buf.DataPointer, out infBuffer);

                if (bFound) {
                    infBuffer.RefCount++;
                    _dicBuffers[buf.DataPointer] = infBuffer;
                } else {
                    _dicBuffers.Add(buf.DataPointer, new BufferInfo() { Buffer = buf, RefCount = 1 });
                }
            }
        }
    }
}
