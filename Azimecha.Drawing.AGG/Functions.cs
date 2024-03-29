﻿using Azimecha.Core;
using Azimecha.Drawing.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG.Interop {
    internal static class Functions {
        private const string AGGWRAP_DLL_NAME = "aggwrap";

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateBitmap(int w, int h);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateBitmapOnBuffer(int w, int h, [In] ref AwBufferInfo infBuffer);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwLoadBitmap(IntPtr pImageFileData, ulong nImageFileSize);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwGetBitmapSize(IntPtr hBitmap, out int nWidth, out int nHeight);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwGetBitmapDataSize(IntPtr hBitmap, out int nStride, out int nFullSize);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwAccessBitmapData(IntPtr hBitmap);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwDuplicateBitmap(IntPtr hBitmap);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeleteBitmap(IntPtr hBitmap);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateSolidBrush(byte r, byte g, byte b, byte a);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateLinearGradientBrush(
            [MarshalAs(UnmanagedType.LPArray)] [In] AwGradientStop[] arrStops, int nStops, float fAngle);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreatePatternBrush(IntPtr hBitmap);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateScaledBrush(IntPtr hBitmap, AwScaleMode mode);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeleteBrush(IntPtr hBrush);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathRectangle(IntPtr hPath, float x, float y, float w, float h);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathRoundedRectangle(IntPtr hPath, float x, float y, float w, 
            float h, float fRadius);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreatePath();
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathCircle(IntPtr hPath, float x, float y, float fRadius);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathEllipse(IntPtr hPath, float x, float y, float w, float h);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathArc(IntPtr hPath, float x, float y, float w, float h, 
            float fStartAngle, float fSweepAngle);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathChord(IntPtr hPath, float x, float y, float w, float h,
            float fStartAngle, float fSweepAngle);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathPieSlice(IntPtr hPath, float x, float y, float w, float h, 
            float fStartAngle, float fSweepAngle);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathLine(IntPtr hPath, float x1, float y1, float x2, float y2);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathTriangle(IntPtr hPath, float x1, float y1, float x2, 
            float y2, float x3, float y3);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathPolyline(IntPtr hPath, 
            [MarshalAs(UnmanagedType.LPArray)] [In] AwPathPoint[] arrPoints, int nPoints);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPathPolygon(IntPtr hPath,
            [MarshalAs(UnmanagedType.LPArray)] [In] AwPathPoint[] arrPoints, int nPoints);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwAddPath(IntPtr hTargetPath, IntPtr hPathToAdd);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwPathMoveTo(IntPtr hPath, float x, float y);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwPathLineTo(IntPtr hPath, float x, float y);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwClosePathShape(IntPtr hPath);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwClearPath(IntPtr hPath);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeletePath(IntPtr hPath);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreatePen(IntPtr hBrush, [In] ref AwPenParams parPen);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwGetPenParams(IntPtr hPen, out AwPenParams parPen);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeletePen(IntPtr hPen);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateContextOnBitmap(IntPtr hBitmap);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwSetDrawQuality(IntPtr hContext, AwQuality qual);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwFillContext(IntPtr hContext, IntPtr hBrush);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwFillPath(IntPtr hContext, IntPtr hBrush, IntPtr hPath);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwFillRectangle(IntPtr hContext, IntPtr hBrush, int x, 
            int y, int w, int h);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwFillText(IntPtr hContext, IntPtr hBrush, IntPtr hFont,
            float x, float y, [MarshalAs(UnmanagedType.LPArray)][In] byte[] arrTextUTF8ZT);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwDrawPath(IntPtr hContext, IntPtr hPen, IntPtr hPath);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwDrawRectangle(IntPtr hContext, IntPtr hPen, int x,
            int y, int w, int h);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwDrawLine(IntPtr hContext, IntPtr hPen, int x1, int y1,
            int x2, int y2);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwBlitImage(IntPtr hDestContext, IntPtr hSrcBitmap, 
            int nSourceX, int nSourceY, int nSourceW, int nSourceH,
            int nDestX, int nDestY, int nDestW, int nDestH);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeleteContext(IntPtr hContext);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateMonochromeBitmapFont(
            in AwFontInfo infFont, in AwBufferInfo infGlyphBuffer, int nGlyphZeroChar, int nGlyphCount, int nGlyphDefault);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwGetTextSize(IntPtr hFont, 
            [MarshalAs(UnmanagedType.LPArray)][In] byte[] arrTextUTF8ZT, out float fWidth, out float fHeight);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate bool AwGetFontInfo(IntPtr hFont, out AwFontInfo infFont);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeleteFont(IntPtr hFont);

        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateFontSetOnTTF(in AwBufferInfo bufData);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate int AwGetFontSetSize(IntPtr hFontSet);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwGetFontNameFromSet(IntPtr hFontSet, int nFont);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate IntPtr AwCreateFontFromSet(IntPtr hFontSet, int nFont, int nHeightPixels);
        [SmartImport] [UnmanagedFunctionPointer(CallingConvention.Winapi)] public delegate void AwDeleteFontSet(IntPtr hFontSet);

        public static readonly SmartLoader Loader = new SmartLoader(typeof(Functions), AGGWRAP_DLL_NAME);
    }
}
