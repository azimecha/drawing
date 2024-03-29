﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public class DrawingContextExtender<TP> : IDrawingContext where TP : IPath, new() {
        private IDrawingContextCore _ctx;

        public DrawingContextExtender(IDrawingContextCore ctx) { _ctx = ctx; }

        public bool HighQuality { get => _ctx.HighQuality; set => _ctx.HighQuality = value; }

        public int Width => _ctx.Width;
        public int Height => _ctx.Height;

        public void Dispose() => _ctx.Dispose();

        public void DrawArc(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            TP path = new TP();
            path.AddArc(x, y, w, h, fStartAngle, fSweepAngle);
            DrawPath(pen, path);
        }

        public void DrawChord(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            TP path = new TP();
            path.AddChord(x, y, w, h, fStartAngle, fSweepAngle);
            DrawPath(pen, path);
        }

        public void DrawCircle(IPen pen, float x, float y, float fRadius) {
            TP path = new TP();
            path.AddCircle(x, y, fRadius);
            DrawPath(pen, path);
        }

        public void DrawEllipse(IPen pen, float x, float y, float w, float h) {
            TP path = new TP();
            path.AddEllipse(x, y, w, h);
            DrawPath(pen, path);
        }

        public void DrawLine(IPen pen, float x1, float y1, float x2, float y2) {
            TP path = new TP();
            path.AddLine(x1, y1, x2, y2);
            DrawPath(pen, path);
        }

        public void DrawLine(IPen pen, int x1, int y1, int x2, int y2) {
            TP path = new TP();
            path.AddLine(x1, y1, x2, y2);
            DrawPath(pen, path);
        }

        public void DrawPath(IPen pen, IPath path) => _ctx.DrawPath(pen, path);

        public void DrawPieSlice(IPen pen, float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            TP path = new TP();
            path.AddPieSlice(x, y, w, h, fStartAngle, fSweepAngle);
            DrawPath(pen, path);
        }

        public void DrawPolygon(IPen pen, IEnumerable<PointF> enuPoints) {
            TP path = new TP();
            path.AddPolygon(enuPoints);
            DrawPath(pen, path);
        }

        public void DrawPolyline(IPen pen, IEnumerable<PointF> enuPoints) {
            TP path = new TP();
            path.AddPolyline(enuPoints);
            DrawPath(pen, path);
        }

        public void DrawRectangle(IPen pen, float x, float y, float w, float h) {
            TP path = new TP();
            path.AddRectangle(x, y, w, h);
            DrawPath(pen, path);
        }

        public void DrawRectangle(IPen pen, int x, int y, int w, int h) => _ctx.DrawRectangle(pen, x, y, w, h);

        public void DrawRoundedRectangle(IPen pen, float x, float y, float w, float h, float fRadius) {
            TP path = new TP();
            path.AddRoundedRectangle(x, y, w, h, fRadius);
            DrawPath(pen, path);
        }

        public void DrawTriangle(IPen pen, float x1, float y1, float x2, float y2, float x3, float y3) {
            TP path = new TP();
            path.AddTriangle(x1, y1, x2, y2, x3, y3);
            DrawPath(pen, path);
        }

        public void Fill(IBrush brFill) => _ctx.Fill(brFill);

        public void FillChord(IBrush brFill, float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            TP path = new TP();
            path.AddChord(x, y, w, h, fStartAngle, fSweepAngle);
            FillPath(brFill, path);
        }

        public void FillCircle(IBrush brFill, float x, float y, float fRadius) {
            TP path = new TP();
            path.AddCircle(x, y, fRadius);
            FillPath(brFill, path);
        }

        public void FillEllipse(IBrush brFill, float x, float y, float w, float h) {
            TP path = new TP();
            path.AddEllipse(x, y, w, h);
            FillPath(brFill, path);
        }

        public void FillPath(IBrush brFill, IPath path) => _ctx.FillPath(brFill, path);

        public void FillPieSlice(IBrush brFill, float x, float y, float w, float h, float fStartAngle, float fSweepAngle) {
            TP path = new TP();
            path.AddPieSlice(x, y, w, h, fStartAngle, fSweepAngle);
            FillPath(brFill, path);
        }

        public void FillPolygon(IBrush brFill, IEnumerable<PointF> enuPoints) {
            TP path = new TP();
            path.AddPolygon(enuPoints);
            FillPath(brFill, path);
        }

        public void FillRectangle(IBrush brFill, float x, float y, float w, float h) {
            TP path = new TP();
            path.AddRectangle(x, y, w, h);
            FillPath(brFill, path);
        }

        public void FillRectangle(IBrush brFill, int x, int y, int w, int h) {
            TP path = new TP();
            path.AddRectangle(x, y, w, h);
            FillPath(brFill, path);
        }

        public void FillRoundedRectangle(IBrush brFill, float x, float y, float w, float h, float fRadius) {
            TP path = new TP();
            path.AddRoundedRectangle(x, y, w, h, fRadius);
            FillPath(brFill, path);
        }

        public void FillTriangle(IBrush brFill, float x1, float y1, float x2, float y2, float x3, float y3) {
            TP path = new TP();
            path.AddTriangle(x1, y1, x2, y2, x3, y3);
            FillPath(brFill, path);
        }

        public void BlitImage(IBitmap bm)
            => BlitImage(bm, 0, 0, Width, Height, 0, 0, bm.Width, bm.Height);

        public void BlitImage(IBitmap bm, int nDestX, int nDestY)
            => BlitImage(bm, nDestX, nDestY, bm.Width, bm.Height, 0, 0, bm.Width, bm.Height);

        public void BlitImage(IBitmap bm, int nDestX, int nDestY, int nDestW, int nDestH)
            => BlitImage(bm, nDestX, nDestY, nDestW, nDestH, 0, 0, bm.Width, bm.Height);

        public void BlitImage(IBitmap bm, int nDestX, int nDestY, int nDestW, int nDestH, int nSourceX, int nSourceY, int nSourceW, int nSourceH)
            => _ctx.BlitImage(bm, nDestX, nDestY, nDestW, nDestH, nSourceX, nSourceY, nSourceW, nSourceH);

        public void FillText(IBrush brFill, IFont font, string strText, float x, float y, Alignment alignHoriz, Alignment alignVert) {
            PointF ptSize = font.GetTextSize(strText);

            switch (alignHoriz) {
                case Alignment.Center:
                    x -= ptSize.X / 2;
                    break;

                case Alignment.Far:
                    x -= ptSize.X;
                    break;
            }

            switch (alignVert) {
                case Alignment.Center:
                    y -= ptSize.Y / 2;
                    break;

                case Alignment.Far:
                    y -= ptSize.Y;
                    break;
            }

            FillText(brFill, font, strText, x, y);
        }

        public void FillText(IBrush brFill, IFont font, string strText, float x, float y, float w, float h, Alignment alignHoriz = Alignment.Near, 
            Alignment alignVert = Alignment.Near, TextWrapping wrap = TextWrapping.WordWrap) 
        {
            TextTokenizer.Token[] arrTokens = TextTokenizer.Tokenize(strText, wrap != TextWrapping.CharWrap);

            TextTokenizer.Token[][] arrLines;
            if (wrap == TextWrapping.None)
                arrLines = TextTokenizer.ToLines(arrTokens);
            else
                arrLines = TextTokenizer.ToWrappedLines(arrTokens, s => font.GetTextSize(s).X, w, wrap == TextWrapping.WordWrap);

            string strWrapped = TextTokenizer.LinesToString(arrLines, "\n");
            PointF ptSize = font.GetTextSize(strWrapped);

            switch (alignHoriz) {
                case Alignment.Center:
                    x += w / 2 - ptSize.X / 2;
                    break;

                case Alignment.Far:
                    x += w - ptSize.X;
                    break;
            }

            switch (alignVert) {
                case Alignment.Center:
                    y += h / 2 - ptSize.Y / 2;
                    break;

                case Alignment.Far:
                    y += h - ptSize.Y;
                    break;
            }

            FillText(brFill, font, strWrapped, x, y);
        }

        public void FillText(IBrush brFill, IFont font, string strText, float x, float y)
            => _ctx.FillText(brFill, font, strText, x, y);
    }
}
