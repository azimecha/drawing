using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public abstract class Brush : IBrush {
        private SafeBrushHandle _hBrush = new SafeBrushHandle();
        internal SafeBrushHandle Handle => _hBrush;
        public override string ToString() => _hBrush.ToString();
    }

    internal class SafeBrushHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.AwDeleteBrush(hObject);
        }
    }
}
