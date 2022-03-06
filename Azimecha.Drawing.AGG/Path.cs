using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class Path : IPath {
        private SafePathHandle _hPath = new SafePathHandle();

        public Path() {
            _hPath.TakeObject(Interop.Functions.AwCreatePath(), true);
            if (!_hPath.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating path");
        }

        public void AddArc(float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            if (!Interop.Functions.AwAddPathArc(_hPath.Handle, x, y, w, h, fStartAngle, fSweepAngle))
                throw new PathOperationFailedException($"Error adding arc to path");
        }

        public void AddChord(float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            if (!Interop.Functions.AwAddPathChord(_hPath.Handle, x, y, w, h, fStartAngle, fSweepAngle))
                throw new PathOperationFailedException($"Error adding chord to path");
        }

        public void AddCircle(float x, float y, float fRadius) {
            if (!Interop.Functions.AwAddPathCircle(_hPath.Handle, x, y, fRadius))
                throw new PathOperationFailedException($"Error adding circle to path");
        }

        public void AddEllipse(float x, float y, float w, float h) {
            if (!Interop.Functions.AwAddPathEllipse(_hPath.Handle, x, y, w, h))
                throw new PathOperationFailedException($"Error adding ellipse to path");
        }

        public void AddLine(float x1, float y1, float x2, float y2) {
            if (!Interop.Functions.AwAddPathLine(_hPath.Handle, x1, y1, x2, y2))
                throw new PathOperationFailedException($"Error adding line to path");
        }

        public void AddPath(IPath path) {
            if (!Interop.Functions.AwAddPath(_hPath.Handle, ((Path)path)._hPath.Handle))
                throw new PathOperationFailedException($"Error concatenating path");
        }

        public void AddPieSlice(float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            if (!Interop.Functions.AwAddPathPieSlice(_hPath.Handle, x, y, w, h, fStartAngle, fSweepAngle))
                throw new PathOperationFailedException($"Error adding slice to path");
        }

        private Interop.AwPathPoint[] ConvertPoints(IEnumerable<PointF> enuPoints)
            => Internal.Utils.ConvertToArray(new Internal.Transformer<PointF, Interop.AwPathPoint>(enuPoints, p => new Interop.AwPathPoint() { x = p.X, y = p.Y }));

        public void AddPolygon(IEnumerable<PointF> enuPoints) {
            Interop.AwPathPoint[] arrPoints = ConvertPoints(enuPoints);
            if (!Interop.Functions.AwAddPathPolygon(_hPath.Handle, arrPoints, arrPoints.Length))
                throw new PathOperationFailedException($"Error adding polygon to path");
        }

        public void AddPolyline(IEnumerable<PointF> enuPoints) {
            Interop.AwPathPoint[] arrPoints = ConvertPoints(enuPoints);
            if (!Interop.Functions.AwAddPathPolyline(_hPath.Handle, arrPoints, arrPoints.Length))
                throw new PathOperationFailedException($"Error adding polyline to path");
        }

        public void AddRectangle(float x, float y, float w, float h) {
            if (!Interop.Functions.AwAddPathRectangle(_hPath.Handle, x, y, w, h))
                throw new PathOperationFailedException($"Error adding rectangle to path");
        }

        public void AddRoundedRectangle(float x, float y, float w, float h, float fRadius) {
            if (!Interop.Functions.AwAddPathRoundedRectangle(_hPath.Handle, x, y, w, h, fRadius))
                throw new PathOperationFailedException($"Error adding rounded rectangle to path");
        }

        public void AddTriangle(float x1, float y1, float x2, float y2, float x3, float y3) {
            if (!Interop.Functions.AwAddPathTriangle(_hPath.Handle, x1, y1, x2, y2, x3, y3))
                throw new PathOperationFailedException($"Error adding triangle to path");
        }

        public void Clear() {
            if (!Interop.Functions.AwClearPath(_hPath.Handle))
                throw new PathOperationFailedException($"Error clearing path");
        }

        public void CloseShape() {
            if (!Interop.Functions.AwClosePathShape(_hPath.Handle))
                throw new PathOperationFailedException($"Error closing shape");
        }

        public Path Duplicate() {
            Path pathNew = new Path();
            pathNew.AddPath(this);
            return pathNew;
        }

        IPath IPath.Duplicate() => Duplicate();

        public void LineTo(float x, float y) {
            if (!Interop.Functions.AwPathLineTo(_hPath.Handle, x, y))
                throw new PathOperationFailedException($"Error adding line to position X={x} Y={y}");
        }

        public void MoveTo(float x, float y) {
            if (!Interop.Functions.AwPathMoveTo(_hPath.Handle, x, y))
                throw new PathOperationFailedException($"Error moving to position X={x} Y={y}");
        }

        public override string ToString() => _hPath.ToString();

        internal SafePathHandle Handle => _hPath;
    }

    internal class SafePathHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.AwDeletePath(hObject);
        }
    }
}
