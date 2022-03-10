﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class MonochromeBitmapFont : Font {
        private MonochromeBitmapFont(int nHeight, char cGlyphZero, char cDefault, GlyphBitmapBuffer bufGlyphs) {
            Interop.AwBufferInfo infBuffer = new Interop.AwBufferInfo() {
                nDataSize = (ulong)bufGlyphs.DataSize,
                pData = bufGlyphs.DataPointer,
                procDestructor = _procGlyphBufferDtor
            };

            infBuffer.tag = GCHandle.ToIntPtr(GCHandle.Alloc(bufGlyphs));

            try {
                Handle.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateMonochromeBitmapFont>()(
                    new Interop.AwFontInfo() { nHeight = (short)nHeight }, infBuffer, cGlyphZero, bufGlyphs.Count, cDefault - cGlyphZero), true);

                if (!Handle.IsHandleValid)
                    throw new ObjectCreationFailedException($"Error creating monochrome bitmap font of height {nHeight} with zero char {cGlyphZero}, "
                        + $"glyph count {bufGlyphs.Count}, default char {cDefault}, and {infBuffer.nDataSize} bytes of data at {infBuffer.pData}");

                infBuffer.tag = IntPtr.Zero;
            } finally {
                if (infBuffer.tag != IntPtr.Zero)
                    GCHandle.FromIntPtr(infBuffer.tag).Free();
            }
        }

        public static MonochromeBitmapFont CreateFixedWidth(char cGlyphZero, int nWidth, int nHeight, BitArray arrData, char cDefault = '?') {
            int nBitsPerGlyph = nWidth * nHeight;
            int nBytesPerGlyph = nBitsPerGlyph / 8 + (((nBitsPerGlyph % 8) != 0) ? 1 : 0);
            int nGlyphs = arrData.Length / nBitsPerGlyph;

            if ((arrData.Length % nBitsPerGlyph) != 0)
                throw new ArgumentException($"The provided bit array is not divisible by the size of the glyph. The size may be incorrect.");

            FontBitmapGlyph[] arrGlyphs = new FontBitmapGlyph[nGlyphs];

            for (int nGlyph = 0; nGlyph < nGlyphs; nGlyph++) {
                FontBitmapGlyph glyph = new FontBitmapGlyph() {
                    Data = new byte[nBytesPerGlyph],
                    Width = (short)nWidth
                };

                BitArray arrThisChar = new BitArray(nBitsPerGlyph);
                for (int nBit = 0; nBit < nBitsPerGlyph; nBit++)
                    arrThisChar[nBit] = arrData[nGlyph * nBitsPerGlyph + nBit];

                arrThisChar.CopyTo(glyph.Data, 0);
                arrGlyphs[nGlyph] = glyph;
            }

            return CreateVariableWidth(cGlyphZero, nHeight, arrGlyphs, cDefault);
        }

        public static MonochromeBitmapFont CreateVariableWidth(char cGlyphZero, int nHeight, IEnumerable<FontBitmapGlyph> enuGlyphs, char cDefault = '?') {
            GlyphBitmapBuffer bufGlyphs = new GlyphBitmapBuffer(enuGlyphs);
            return new MonochromeBitmapFont(nHeight, cGlyphZero, cDefault, bufGlyphs);
        }

        private static readonly Interop.AwDataDestructor _procGlyphBufferDtor = GlyphBufferDestructor;
        private static void GlyphBufferDestructor(IntPtr pIgnored, IntPtr hBuffer) {
            GCHandle gchBuffer = GCHandle.FromIntPtr(hBuffer);
            ((GlyphBitmapBuffer)gchBuffer.Target).Dispose();
            gchBuffer.Free();
        }
    }

    public struct FontBitmapGlyph {
        public short Width;
        public byte[] Data;
    }

    internal class GlyphBitmapBuffer : IDataBuffer {
        private UnmanagedDataBuffer<Interop.AwFontGlyphInfo> _bufInfo;
        private UnmanagedDataBuffer<byte> _bufData;

        public GlyphBitmapBuffer(IEnumerable<FontBitmapGlyph> enuGlyphs) {
            int nCount = 0, nTotalDataSize = 0;
            foreach (FontBitmapGlyph glyph in enuGlyphs) {
                nCount++;
                nTotalDataSize += glyph.Data.Length;
            }

            _bufInfo = new UnmanagedDataBuffer<Interop.AwFontGlyphInfo>(nCount);
            _bufData = new UnmanagedDataBuffer<byte>(nTotalDataSize);

            int nGlyph = 0, nDataOffset = 0;
            foreach (FontBitmapGlyph glyph in enuGlyphs) {
                _bufInfo[nGlyph] = new Interop.AwFontGlyphInfo() {
                    nWidthPixels = glyph.Width,
                    pData = (IntPtr)((long)_bufData.DataPointer + nDataOffset)
                };

                Marshal.Copy(glyph.Data, 0, _bufInfo[nGlyph].pData, glyph.Data.Length);

                nGlyph++;
                nDataOffset += glyph.Data.Length;
            }
        }

        public IntPtr DataPointer => _bufInfo.DataPointer;
        public long DataSize => _bufInfo.DataSize;

        public unsafe int Count => (int)(_bufInfo.DataSize / sizeof(Interop.AwFontGlyphInfo));

        public void Dispose() {
            _bufInfo?.Dispose();
            _bufData?.Dispose();
        }
    }

    internal class UnmanagedDataBuffer<T> : IDataBuffer where T : unmanaged {
        private SafeGlobalHandle _hData = new SafeGlobalHandle();
        private int _nCount = 0;

        public unsafe UnmanagedDataBuffer(int nObjectCount) {
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
    }

    internal class SafeGlobalHandle : Internal.SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Marshal.FreeHGlobal(hObject);
        }
    }
}
