using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class PatternBrush : Brush, IBitmapBrush {
        public PatternBrush(Bitmap bmPattern) {
            Handle.TakeObject(Interop.Functions.AwCreatePatternBrush(bmPattern.Handle.Handle), true);
            if (!Handle.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating pattern brush from bitmap {bmPattern}");
        }

        public ScaleMode BitmapScaleMode => ScaleMode.Tile;
    }
}
