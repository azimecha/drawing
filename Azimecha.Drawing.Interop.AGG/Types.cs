using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.AGG.Interop {
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

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void AwDataDestructor(IntPtr pData);
}
