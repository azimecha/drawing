using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG.Interop {
    internal static class Functions {
        private const string AGGWRAP_DLL_NAME = "aggwrap";

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateBitmap(int w, int h);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateBitmapOnBuffer(int w, int h, [In] ref AwBufferInfo infBuffer);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwGetBitmapSize(IntPtr hBitmap, out int nWidth, out int nHeight);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwGetBitmapDataSize(IntPtr hBitmap, out int nStride, out int nFullSize);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwAccessBitmapData(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwDuplicateBitmap(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeleteBitmap(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateSolidBrush(byte r, byte g, byte b, byte a);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateLinearGradientBrush([MarshalAs(UnmanagedType.LPArray)] [In] AwGradientStop[] arrStops, 
            int nStops, float fAngle);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreatePatternBrush(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateScaledBrush(IntPtr hBitmap, AwScaleMode mode);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeleteBrush(IntPtr hBrush);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathRectangle(IntPtr hPath, float x, float y, float w, float h);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathRoundedRectangle(IntPtr hPath, float x, float y, float w, float h,
            float fRadius);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreatePath();

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathCircle(IntPtr hPath, float x, float y, float fRadius);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathEllipse(IntPtr hPath, float x, float y, float w, float h);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathArc(IntPtr hPath, float x, float y, float w, float h, float fStartAngle,
            float fSweepAngle);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathChord(IntPtr hPath, float x, float y, float w, float h, float fStartAngle,
            float fSweepAngle);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathPieSlice(IntPtr hPath, float x, float y, float w, float h, float fStartAngle,
            float fSweepAngle);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathLine(IntPtr hPath, float x1, float y1, float x2, float y2);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathTriangle(IntPtr hPath, float x1, float y1, float x2, float y2, float x3,
            float y3);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathPolyline(IntPtr hPath, [MarshalAs(UnmanagedType.LPArray)] [In] AwPathPoint[] arrPoints, int nPoints);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPathPolygon(IntPtr hPath, [MarshalAs(UnmanagedType.LPArray)][In] AwPathPoint[] arrPoints, int nPoints);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwAddPath(IntPtr hTargetPath, IntPtr hPathToAdd);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwPathMoveTo(IntPtr hPath, float x, float y);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwPathLineTo(IntPtr hPath, float x, float y);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwClosePathShape(IntPtr hPath);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwClearPath(IntPtr hPath);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeletePath(IntPtr hPath);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreatePen(IntPtr hBrush, [In] ref AwPenParams parPen);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwGetPenParams(IntPtr hPen, out AwPenParams parPen);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeletePen(IntPtr hPen);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwCreateContextOnBitmap(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwSetDrawQuality(IntPtr hContext, AwQuality qual);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwFillContext(IntPtr hContext, IntPtr hBrush);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwFillPath(IntPtr hContext, IntPtr hBrush, IntPtr hPath);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwFillRectangle(IntPtr hContext, IntPtr hBrush, int x, int y, int w, int h);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwDrawPath(IntPtr hContext, IntPtr hPen, IntPtr hPath);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwDrawRectangle(IntPtr hContext, IntPtr hPen, int x, int y, int w, int h);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwDrawLine(IntPtr hContext, IntPtr hPen, int x1, int y1, int x2, int y2);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeleteContext(IntPtr hContext);
    }
}
