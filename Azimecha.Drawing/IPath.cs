using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IPath {
        void AddRectangle(float x, float y, float w, float h);
        void AddRoundedRectangle(float x, float y, float w, float h, float fRadius);
        void AddCircle(float x, float y, float fRadius);
        void AddEllipse(float x, float y, float w, float h);
        void AddArc(float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void AddChord(float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void AddPieSlice(float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void AddLine(float x1, float y1, float x2, float y2);
        void AddTriangle(float x1, float y1, float x2, float y2, float x3, float y3);
        void AddPolyline(IEnumerable<PointF> enuPoints);
        void AddPolygon(IEnumerable<PointF> enuPoints);
        void AddPath(IPath path);

        void MoveTo(float x, float y);
        void LineTo(float x, float y);
        void CloseShape();
        void Clear();

        IPath Duplicate();
    }
}
