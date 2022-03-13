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
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwGetBitmapSize>()(hBitmap.Handle, out _nWidth, out _nHeight))
                throw new InfoQueryFailedException($"Unable to get size of bitmap {hBitmap}");

            _hBitmap = hBitmap;
        }

        public Bitmap(int w, int h) {
            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            hBitmap.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateBitmap>()(w, h), true);
            if (!hBitmap.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating empty {w}x{h} bitmap");

            _hBitmap = hBitmap;
            _nWidth = w;
            _nHeight = h;
        }

        public Bitmap(int w, int h, IDataBuffer buf, bool bDisposeBuffer = true) {
            int nExpectedLength = w * h * 4;
            if (buf.DataSize < nExpectedLength)
                throw new ArgumentException($"Data buffer too small for {w}x{h} bitmap: expected {nExpectedLength} bytes, got {buf.DataSize}");

            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            BufferUtils.TryPassOwnership(buf, bDisposeBuffer, infBuffer => {
                hBitmap.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateBitmapOnBuffer>()(w, h, ref infBuffer), true);
                if (!hBitmap.IsHandleValid)
                    throw new ObjectCreationFailedException($"Error creating {w}x{h} bitmap");
            });

            _hBitmap = hBitmap;
            _nWidth = w;
            _nHeight = h;
        }

        public int Width => _nWidth;
        public int Height => _nHeight;
        public long Stride => _nWidth * 4;
        public long DataSize => Stride * _nHeight;
        public PixelFormat Format => PixelFormat.ARGB32Premul;
        internal SafeBitmapHandle Handle => _hBitmap;

        public static Bitmap CreateOnArray(int w, int h, byte[] arrData) {
            int nExpectedLength = w * h * 4;
            if (arrData.Length != nExpectedLength)
                throw new ArgumentException($"Incorrect data size for {w}x{h} bitmap: expected {nExpectedLength} bytes, got {arrData.Length}");
            return new Bitmap(w, h, new Internal.PinnedArrayDataBuffer<byte>(arrData));
        }

        public static Bitmap CreateOnArray(int w, int h, uint[] arrData) {
            int nExpectedLength = w * h;
            if (arrData.Length != nExpectedLength)
                throw new ArgumentException($"Incorrect data size for {w}x{h} bitmap: expected {nExpectedLength} pixels, got {arrData.Length}");
            return new Bitmap(w, h, new Internal.PinnedArrayDataBuffer<uint>(arrData));
        }

        public static Bitmap FromFile(string strFilePath) {
            using (IDataBuffer bufMappedFile = Internal.MemoryMapping.MapFileReadOnly(strFilePath, false))
                return FromFile(bufMappedFile);
        }

        public static Bitmap FromFile(IDataBuffer bufFileData) {
            SafeBitmapHandle hBitmap = new SafeBitmapHandle();
            hBitmap.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwLoadBitmap>()(bufFileData.DataPointer, (ulong)bufFileData.DataSize),
                true);

            if (!hBitmap.IsHandleValid)
                throw new FormatException($"Error loading {bufFileData.DataSize} byte image at {bufFileData.DataPointer}. "
                    + "The image may be corrupt, or its format may not be supported.");

            return new Bitmap(hBitmap);
        }

        public Bitmap Duplicate() {
            SafeBitmapHandle hBitmap = new SafeBitmapHandle();

            hBitmap.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwDuplicateBitmap>()(_hBitmap.Handle), true);
            if (!hBitmap.IsHandleValid)
                throw new ObjectCreationFailedException($"Error duplicating bitmap {_hBitmap}");

            return new Bitmap(hBitmap);
        }

        IBitmap IBitmap.Duplicate() => Duplicate();

        public void Dispose() {
            _hBitmap.Dispose();
        }

        public override string ToString() => _hBitmap.ToString();
        
        internal IntPtr GetDataPointer() {
            IntPtr pData = Interop.Functions.Loader.GetMethod<Interop.Functions.AwAccessBitmapData>()(_hBitmap.Handle);
            if (pData == IntPtr.Zero)
                throw new DataAccessException($"Error accessing data of bitmap {_hBitmap}");
            return pData;
        }

        public IBitmapDataAccessor AccessData(bool bRead, bool bWrite)
            => new BitmapDataAccessor(this);

        public byte[] ReadData() {
            byte[] arrData = new byte[DataSize];
            Marshal.Copy(GetDataPointer(), arrData, 0, arrData.Length);
            return arrData;
        }

        public void WriteData(byte[] arrData) {
            if (arrData.Length != DataSize)
                throw new ArgumentException($"Array size {arrData.Length} does not match bitmap data size {DataSize}");
            Marshal.Copy(arrData, 0, GetDataPointer(), (int)DataSize);
        }

        public IDrawingContext CreateContext() => DrawingContext.CreateFullContext(this);
    }

    internal class SafeBitmapHandle : Internal.SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeleteBitmap>()(hObject);
        }
    }

    internal class BitmapDataAccessor : IBitmapDataAccessor {
        private Bitmap _bm;

        public BitmapDataAccessor(Bitmap bm) {
            Pointer = bm.GetDataPointer();
            _bm = bm;
        }

        public IntPtr Pointer { get; private set; }
        public long Size => _bm.DataSize;
        public void Dispose() { }
    }
}
