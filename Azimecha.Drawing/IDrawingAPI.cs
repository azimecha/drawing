using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IDrawingAPI {
        IBitmap CreateBitmap(int nWidth, int nHeight);
        IBitmap CreateBitmapOnData(int nWidth, int nHeight, IDataBuffer bufData);
        IBitmap CreateBitmapFromData(int nWidth, int nHeight, IDataBuffer bufData);

        IBitmap CreateBitmapFromFile(string strFilePath);
        IBitmap CreateBitmapFromFile(IDataBuffer bufFileData);

        ISolidBrush CreateSolidBrush(Color clr);
        IBitmapBrush CreateBitmapBrush(IBitmap bitmap, ScaleMode modeScale);
        ILinearGradientBrush CreateLinearGradientBrush(IEnumerable<KeyValuePair<float, Color>> enuStops, float fAngle);

        IFont CreateClassicBitmapFont(char cGlyphZero, int nWidth, int nHeight, BitArray arrData, char cDefault = '?',
            bool bReverseBitOrder = false);
        IFont CreateVariableWidthBitmapFont(char cGlyphZero, int nHeight, IEnumerable<FontBitmapGlyph> enuGlyphs, char cDefault = '?',
            bool bReverseBitOrder = false);

        IFontSet CreateTrueTypeFontSet(string strFilePath);
        IFontSet CreateTrueTypeFontSet(IDataBuffer bufData);
    }
}
