using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IBrush {
        IPen CreatePen(float fThickness);
    }

    public interface ISolidBrush : IBrush {
        Color FillColor { get; }
    }

    public interface IGradientBrush : IBrush {
        IEnumerable<KeyValuePair<float, Color>> GradientStops { get; }
    }

    public interface ILinearGradientBrush : IGradientBrush {
        float Angle { get; }
    }

    public interface IBitmapBrush : IBrush {
        ScaleMode BitmapScaleMode { get; }
    }
}
