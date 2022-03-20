using Azimecha.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class DrawingAPI : IDrawingAPI {
        public IBitmap CreateBitmap(int nWidth, int nHeight)
            => new Bitmap(nWidth, nHeight);

        public IBitmapBrush CreateBitmapBrush(IBitmap bitmap, ScaleMode modeScale)
            => (modeScale == ScaleMode.Tile) ? (IBitmapBrush)new PatternBrush((Bitmap)bitmap) : (IBitmapBrush)new ScaledBrush((Bitmap)bitmap, modeScale);

        public IBitmap CreateBitmapFromData(int nWidth, int nHeight, IDataBuffer bufData)
            => new Bitmap(nWidth, nHeight, HGlobalDataBuffer<byte>.FromExisting(bufData));

        public IBitmap CreateBitmapOnData(int nWidth, int nHeight, IDataBuffer bufData)
            => new Bitmap(nWidth, nHeight, bufData);

        public IFont CreateClassicBitmapFont(char cGlyphZero, int nWidth, int nHeight, BitArray arrData, char cDefault = '?', bool bReverseBitOrder = false)
            => MonochromeBitmapFont.CreateFixedWidth(cGlyphZero, nWidth, nHeight, arrData, cDefault, bReverseBitOrder);

        public ILinearGradientBrush CreateLinearGradientBrush(IEnumerable<KeyValuePair<float, Color>> enuStops, float fAngle)
            => new LinearGradientBrush(enuStops, fAngle);

        public ISolidBrush CreateSolidBrush(Color clr)
            => new SolidBrush(clr);

        public IFontSet LoadTrueTypeFonts(string strFilePath)
            => new TrueTypeFontFile(strFilePath);

        public IFontSet LoadTrueTypeFonts(IDataBuffer bufData)
            => new TrueTypeFontFile(bufData);

        public IFont CreateVariableWidthBitmapFont(char cGlyphZero, int nHeight, IEnumerable<FontBitmapGlyph> enuGlyphs, char cDefault = '?', bool bReverseBitOrder = false)
            => MonochromeBitmapFont.CreateVariableWidth(cGlyphZero, nHeight, enuGlyphs, cDefault, bReverseBitOrder);

        public IBitmap LoadBitmap(string strFilePath)
            => Bitmap.FromFile(strFilePath);

        public IBitmap LoadBitmap(Stream stmFileData) {
            using (IDataBuffer bufData = HGlobalDataBuffer<byte>.FromStream(stmFileData))
                return Bitmap.FromFile(bufData);
        }

        public IBitmap LoadBitmap(IDataBuffer bufFileData)
            => Bitmap.FromFile(bufFileData);
    }
}
