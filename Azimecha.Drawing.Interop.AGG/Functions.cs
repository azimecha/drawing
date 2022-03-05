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
        public static extern IntPtr AwCreateBitmapOnBuffer(int w, int h, IntPtr pData, AwDataDestructor procDtor);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwGetBitmapSize(IntPtr hBitmap, out int nWidth, out int nHeight);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern bool AwGetBitmapDataSize(IntPtr hBitmap, out int nStride, out int nFullSize);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern IntPtr AwDuplicateBitmap(IntPtr hBitmap);

        [DllImport(AGGWRAP_DLL_NAME)]
        public static extern void AwDeleteBitmap(IntPtr hBitmap);
    }
}
