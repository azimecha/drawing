using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class SolidBrush : Brush, ISolidBrush {
        public SolidBrush(Color clr) : this(clr.R, clr.G, clr.B, clr.A) { }

        public SolidBrush(byte r, byte g, byte b, byte a = 255) {
            Handle.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateSolidBrush>()(r, g, b, a), true);
            if (!Handle.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating solid brush with color R={r} G={g} B={b} A={a}");

            FillColor = new Color(r, g, b, a);
        }

        public Color FillColor { get; private set; }
    }
}
