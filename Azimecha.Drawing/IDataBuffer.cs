using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IDataBuffer : IDisposable {
        IntPtr DataPointer { get; }
        long DataSize { get; }
    }
}
