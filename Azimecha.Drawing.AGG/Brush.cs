using Azimecha.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public abstract class Brush : IBrush {
        private SafeBrushHandle _hBrush = new SafeBrushHandle();
        internal SafeBrushHandle Handle => _hBrush;
        public override string ToString() => _hBrush.ToString();

        public IPen CreatePen(float fThickness)
            => new Pen(this, fThickness);
    }


    internal class SafeBrushHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeleteBrush>()(hObject);
        }
    }
}
