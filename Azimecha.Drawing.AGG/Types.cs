using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG.Interop {
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void AwDataDestructor(IntPtr pData, IntPtr tag);

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwBufferInfo {
        public IntPtr pData;
        public ulong nDataSize;
        public AwDataDestructor procDestructor;
        public IntPtr tag;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwColor {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }

    internal enum AwScaleMode : int {
        OriginalSize,
        Fit,
        Fill,
        Stretch
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwPathPoint {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwGradientStop {
        public float pos;
        public AwColor clr;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwPenParams {
        public float fThickness;
        public bool bRounded;
    }

    internal enum AwQuality {
        Good, Fast
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwFontInfo {
        public short nHeight;
        public float fAscent;
        public float fInternalLeading;
        public float fExternalLeading;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AwFontGlyphInfo {
        public short nWidthPixels;
        public IntPtr pData;
    }
}
