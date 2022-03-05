using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class Bitmap : IBitmap {
        private SafeBitmapHandle _hBitmap;
        private int _nWidth, _nHeight;

        internal Bitmap(SafeBitmapHandle hBitmap) {
            if (!Interop.Functions.AwGetBitmapSize(hBitmap.Handle, out _nWidth, out _nHeight))
                throw new InfoQueryFailedException($"Unable to get size of bitmap {hBitmap}");

            _hBitmap = hBitmap;
        }

        public Bitmap(int w, int h) {
            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            hBitmap.TakeObject(Interop.Functions.AwCreateBitmap(w, h), true);
            if (!hBitmap.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating empty {w}x{h} bitmap");

            _hBitmap = hBitmap;
            _nWidth = w;
            _nHeight = h;
        }

        public Bitmap(int w, int h, IBitmapDataBuffer buf, bool bDisposeBuffer = true) {
            int nExpectedLength = w * h * 4;
            if (buf.DataSize < nExpectedLength)
                throw new ArgumentException($"Data buffer too small for {w}x{h} bitmap: expected {nExpectedLength} bytes, got {buf.DataSize}");

            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            GCHandle? gchBufferObject = GCHandle.Alloc(buf);
            try {
                Interop.AwBufferInfo infBuffer = new Interop.AwBufferInfo() {
                    nDataSize = (ulong)buf.DataSize,
                    pData = buf.DataPointer,
                    procDestructor = bDisposeBuffer ? _procDisposeDtor : _procNoDisposeDtor,
                    tag = GCHandle.ToIntPtr(gchBufferObject.Value)
                };

                hBitmap.TakeObject(Interop.Functions.AwCreateBitmapOnBuffer(w, h, ref infBuffer), true);
                if (!hBitmap.IsHandleValid)
                    throw new ObjectCreationFailedException($"Error creating {w}x{h} bitmap");

                gchBufferObject = null;
            } finally {
                gchBufferObject?.Free();
            }

            _hBitmap = hBitmap;
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
            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            hBitmap.TakeObject(Interop.Functions.AwDuplicateBitmap(_hBitmap.Handle), true);
            if (!hBitmap.IsHandleValid)
                throw new ObjectCreationFailedException($"Error duplicating bitmap {_hBitmap}");

            return new Bitmap(hBitmap);
        }

        IBitmap IBitmap.Duplicate() => Duplicate();

        public void Dispose() {
            _hBitmap.Dispose();
        }

        private static readonly Interop.AwDataDestructor _procDisposeDtor = DataDisposeDtorProc;
        private static readonly Interop.AwDataDestructor _procNoDisposeDtor = DataNoDisposeDtorProc;

        private static void DataDisposeDtorProc(IntPtr pDataIgnored, IntPtr hGCHandle) {
            GCHandle gchBufferObject = GCHandle.FromIntPtr(hGCHandle);
            (gchBufferObject.Target as IDisposable)?.Dispose();
            gchBufferObject.Free();
        }

        private static void DataNoDisposeDtorProc(IntPtr pDataIgnored, IntPtr hGCHandle) {
            GCHandle gchBufferObject = GCHandle.FromIntPtr(hGCHandle);
            gchBufferObject.Free();
        }
    }

    internal class SafeBitmapHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.AwDeleteBitmap(hObject);
        }
    }
}
