using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IDrawingContextCore : IDisposable {
        bool HighQuality { get; set; }
        
        int Width { get; }
        int Height { get; }

        void Fill(IBrush brFill);
        void FillPath(IBrush brFill, IPath path);
        void FillRectangle(IBrush brFill, int x, int y, int w, int h);
        void FillText(IBrush brFill, IFont font, string strText, float x, float y); // top left

        void DrawPath(IPen pen, IPath path);
        void DrawRectangle(IPen pen, int x, int y, int w, int h);
        void DrawLine(IPen pen, int x1, int y1, int x2, int y2);

        void BlitImage(IBitmap bm, int nDestX, int nDestY, int nDestW, int nDestH, int nSourceX, int nSourceY, int nSourceW, int nSourceH);
    }

    public interface IDrawingContext : IDrawingContextCore {
        void FillRectangle(IBrush brFill, float x, float y, float w, float h);
        void FillRoundedRectangle(IBrush brFill, float x, float y, float w, float h, float fRadius);
        void FillCircle(IBrush brFill, float x, float y, float fRadius);
        void FillEllipse(IBrush brFill, float x, float y, float w, float h);
        void FillChord(IBrush brFill, float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void FillPieSlice(IBrush brFill, float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void FillTriangle(IBrush brFill, float x1, float y1, float x2, float y2, float x3, float y3);
        void FillPolygon(IBrush brFill, IEnumerable<PointF> enuPoints);
        void FillText(IBrush brFill, IFont font, string strText, float x, float y, Alignment alignHoriz, Alignment alignVert);
        void FillText(IBrush brFill, IFont font, string strText, float x, float y, float w, float h, Alignment alignHoriz = Alignment.Near, 
            Alignment alignVert = Alignment.Near, TextWrapping wrap = TextWrapping.WordWrap);

        void DrawRectangle(IPen pen, float x, float y, float w, float h);
        void DrawRoundedRectangle(IPen pen, float x, float y, float w, float h, float fRadius);
        void DrawCircle(IPen pen, float x, float y, float fRadius);
        void DrawEllipse(IPen pen, float x, float y, float w, float h);
        void DrawArc(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void DrawChord(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void DrawPieSlice(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle);
        void DrawLine(IPen pen, float x1, float y1, float x2, float y2);
        void DrawTriangle(IPen pen, float x1, float y1, float x2, float y2, float x3, float y3);
        void DrawPolyline(IPen pen, IEnumerable<PointF> enuPoints);
        void DrawPolygon(IPen pen, IEnumerable<PointF> enuPoints);

        void BlitImage(IBitmap bm);
        void BlitImage(IBitmap bm, int nDestX, int nDestY);
        void BlitImage(IBitmap bm, int nDestX, int nDestY, int nDestW, int nDestH);
    }
}
