using Azimecha.Drawing.Internal;
using System;
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

        public static MonochromeBitmapFont CreateFixedWidth(char cGlyphZero, int nWidth, int nHeight, BitArray arrData, char cDefault = '?',
            bool bReverseBitOrder = false)
        {
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

            return CreateVariableWidth(cGlyphZero, nHeight, arrGlyphs, cDefault, bReverseBitOrder);
        }

        public static MonochromeBitmapFont CreateVariableWidth(char cGlyphZero, int nHeight, IEnumerable<FontBitmapGlyph> enuGlyphs, char cDefault = '?',
            bool bReverseBitOrder = false) 
        {
            if (bReverseBitOrder)
                enuGlyphs = new Internal.Transformer<FontBitmapGlyph, FontBitmapGlyph>(enuGlyphs, ReverseBitOrder);

            GlyphBitmapBuffer bufGlyphs = new GlyphBitmapBuffer(enuGlyphs);
            return new MonochromeBitmapFont(nHeight, cGlyphZero, cDefault, bufGlyphs);
        }

        private static FontBitmapGlyph ReverseBitOrder(FontBitmapGlyph glyph) {
            byte[] arrNewData = new byte[glyph.Data.Length];

            for (int nByte = 0; nByte < glyph.Data.Length; nByte++)
                arrNewData[nByte] = Internal.Utils.ReverseBits(glyph.Data[nByte]);

            glyph.Data = arrNewData;
            return glyph;
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
        private HGlobalDataBuffer<Interop.AwFontGlyphInfo> _bufInfo;
        private HGlobalDataBuffer<byte> _bufData;

        public GlyphBitmapBuffer(IEnumerable<FontBitmapGlyph> enuGlyphs) {
            int nCount = 0, nTotalDataSize = 0;
            foreach (FontBitmapGlyph glyph in enuGlyphs) {
                nCount++;
                nTotalDataSize += glyph.Data.Length;
            }

            _bufInfo = new HGlobalDataBuffer<Interop.AwFontGlyphInfo>(nCount);
            _bufData = new HGlobalDataBuffer<byte>(nTotalDataSize);

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
}
