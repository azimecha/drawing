﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IBitmap : IDisposable {
        int Width { get; }
        int Height { get; }
        long Stride { get; }
        long DataSize { get; }
        PixelFormat Format { get; }

        IBitmap Duplicate();
    }

    public enum PixelFormat {
        ARGB32Premul
    }
}
