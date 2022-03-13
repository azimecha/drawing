using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class DrawingAPI : IDrawingAPI {
        public IBitmap CreateBitmap(int nWidth, int nHeight)
            => new Bitmap(nWidth, nHeight);

        public IBitmapBrush CreateBitmapBrush(IBitmap bitmap, ScaleMode modeScale)
            => (modeScale == ScaleMode.Tile) ? (IBitmapBrush)new PatternBrush((Bitmap)bitmap) : (IBitmapBrush)new ScaledBrush((Bitmap)bitmap, modeScale);

        public IBitmap CreateBitmapFromData(int nWidth, int nHeight, IDataBuffer bufData)
            => new Bitmap(nWidth, nHeight, Internal.HGlobalDataBuffer<byte>.FromExisting(bufData));

        public IBitmap CreateBitmapFromFile(string strFilePath) {
            throw new NotImplementedException();
        }

        public IBitmap CreateBitmapFromFile(IDataBuffer bufFileData) {
            throw new NotImplementedException();
        }

        public IBitmap CreateBitmapOnData(int nWidth, int nHeight, IDataBuffer bufData)
            => new Bitmap(nWidth, nHeight, bufData);

        public IFont CreateClassicBitmapFont(char cGlyphZero, int nWidth, int nHeight, BitArray arrData, char cDefault = '?', bool bReverseBitOrder = false)
            => MonochromeBitmapFont.CreateFixedWidth(cGlyphZero, nWidth, nHeight, arrData, cDefault, bReverseBitOrder);

        public ILinearGradientBrush CreateLinearGradientBrush(IEnumerable<KeyValuePair<float, Color>> enuStops, float fAngle)
            => new LinearGradientBrush(enuStops, fAngle);

        public ISolidBrush CreateSolidBrush(Color clr)
            => new SolidBrush(clr);

        public IFontSet CreateTrueTypeFontSet(string strFilePath)
            => new TrueTypeFontFile(strFilePath);

        public IFontSet CreateTrueTypeFontSet(IDataBuffer bufData)
            => new TrueTypeFontFile(bufData);

        public IFont CreateVariableWidthBitmapFont(char cGlyphZero, int nHeight, IEnumerable<FontBitmapGlyph> enuGlyphs, char cDefault = '?', bool bReverseBitOrder = false)
            => MonochromeBitmapFont.CreateVariableWidth(cGlyphZero, nHeight, enuGlyphs, cDefault, bReverseBitOrder);
    }
}
