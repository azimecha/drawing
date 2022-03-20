using Azimecha.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IFontSet : IArrayLike<IFontPrototype> {}

    public interface IFontPrototype {
        string Name { get; }
        IFont CreateFont(int nHeightPixels);
    }
}
