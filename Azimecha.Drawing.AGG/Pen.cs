using Azimecha.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class Pen : IPen {
        private SafePenHandle _hPen = new SafePenHandle();

        public Pen(Brush brush, float fThickness) {
            Interop.AwPenParams parPen = new Interop.AwPenParams() {
                    fThickness = fThickness
            };

            _hPen.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreatePen>()(brush.Handle.Handle, ref parPen), true);
            if (!_hPen.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating pen of thickness {fThickness} from brush {brush}");

            Thickness = fThickness;
        }

        internal SafePenHandle Handle => _hPen;
        
        public float Thickness { get; private set; }

        public override string ToString() => _hPen.ToString();
    }

    internal class SafePenHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hPen) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeletePen>()(hPen);
        }
    }
}
