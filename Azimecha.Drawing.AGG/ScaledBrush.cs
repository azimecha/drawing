using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class ScaledBrush : Brush, IBitmapBrush {
        public ScaledBrush(Bitmap bm, ScaleMode mode) {
            Interop.AwScaleMode modeInternal;

            switch (mode) {
                case ScaleMode.NoScaling:
                    modeInternal = Interop.AwScaleMode.OriginalSize;
                    break;

                case ScaleMode.Fit:
                    modeInternal = Interop.AwScaleMode.Fit;
                    break;

                case ScaleMode.Fill:
                    modeInternal = Interop.AwScaleMode.Fill;
                    break;

                case ScaleMode.Stretch:
                    modeInternal = Interop.AwScaleMode.Stretch;
                    break;

                default:
                    throw new NotSupportedException($"ScaledBrush does not support the {mode} scaling mode");
            }

            Handle.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateScaledBrush>()(bm.Handle.Handle, modeInternal), true);
            if (!Handle.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating ScaledBrush from bitmap {bm} using scale mode {mode}");

            BitmapScaleMode = mode;
        }

        public ScaleMode BitmapScaleMode { get; private set; }
    }
}
