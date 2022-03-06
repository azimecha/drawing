using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IBitmapDataAccessor : IDisposable {
        IntPtr Pointer { get; }
        long Size { get; }
    }
}
