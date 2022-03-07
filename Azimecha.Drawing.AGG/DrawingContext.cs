using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    internal class DrawingContext : IDrawingContextCore {
        private Bitmap _bmTarget;
        private SafeContextHandle _hContext = new SafeContextHandle();
        private bool _bHighQuality = true;

        public DrawingContext(Bitmap bmTarget) {
            _hContext.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateContextOnBitmap>()(bmTarget.Handle.Handle), true);
            if (!_hContext.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating drawing context on bitmap {bmTarget}");

            _bmTarget = bmTarget;
        }

        public static IDrawingContext CreateFullContext(Bitmap bmTarget)
            => new Internal.DrawingContextExtender<Path>(new DrawingContext(bmTarget));

        public bool HighQuality { 
            get => _bHighQuality;
            set {
                if (_bHighQuality != value) {
                    if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwSetDrawQuality>()(_hContext.Handle, value ? Interop.AwQuality.Good : Interop.AwQuality.Fast))
                        throw new DrawOperationFailedException("Error setting draw quality to " + (value ? "high" : "low") + " quality");
                    _bHighQuality = value;
                }
            }
        }

        public int Width => _bmTarget.Width;
        public int Height => _bmTarget.Height;

        public void Dispose() {
            _hContext.Dispose();
            _bmTarget = null;
        }

        public void DrawLine(IPen pen, int x1, int y1, int x2, int y2) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwDrawLine>()(_hContext.Handle, ((Pen)pen).Handle.Handle, x1, y1, x2, y2))
                throw new DrawOperationFailedException($"Error drawing line from X={x1} Y={y1} to X={x2} Y={y2} using pen {pen}");
        }

        public void DrawPath(IPen pen, IPath path) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwDrawPath>()(_hContext.Handle, ((Pen)pen).Handle.Handle, ((Path)path).Handle.Handle))
                throw new DrawOperationFailedException($"Error drawing path {path} using pen {pen}");
        }

        public void DrawRectangle(IPen pen, int x, int y, int w, int h) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwDrawRectangle>()(_hContext.Handle, ((Pen)pen).Handle.Handle, x, y, w, h))
                throw new DrawOperationFailedException($"Error drawing rectangle of size W={w} H={h} at location X={x} Y={y} using pen {pen}");
        }

        public void Fill(IBrush brFill) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwFillContext>()(_hContext.Handle, ((Brush)brFill).Handle.Handle))
                throw new DrawOperationFailedException($"Error filling context using brush {brFill}");
        }

        public void FillPath(IBrush brFill, IPath path) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwFillPath>()(_hContext.Handle, ((Brush)brFill).Handle.Handle, ((Path)path).Handle.Handle))
                throw new DrawOperationFailedException($"Error filling path {path} using brush {brFill}");
        }

        public void FillRectangle(IBrush brFill, int x, int y, int w, int h) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwDrawRectangle>()(_hContext.Handle, ((Brush)brFill).Handle.Handle, x, y, w, h))
                throw new DrawOperationFailedException($"Error filling rectangle of size W={w} H={h} at location X={x} Y={y} using brush {brFill}");
        }

        public void BlitImage(IBitmap bm, int nDestX, int nDestY, int nDestW, int nDestH, int nSourceX, int nSourceY, int nSourceW, int nSourceH) {
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwBlitImage>()(_hContext.Handle, ((Bitmap)bm).Handle.Handle, nSourceX, nSourceY,
                    nSourceW, nSourceH, nDestX, nDestY, nDestW, nDestH))
                throw new DrawOperationFailedException($"Error blitting bitmap {bm}");
        }
    }

    internal class SafeContextHandle : Internal.SafeHandle {
        protected override void CloseObjectHandle(IntPtr hContext) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeleteContext>()(hContext);
        }
    }
}
