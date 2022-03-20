using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public interface IArrayLike<T> : IEnumerable<T> {
        int Count { get; }
        T this[int nIndex] { get; }
    }
}
