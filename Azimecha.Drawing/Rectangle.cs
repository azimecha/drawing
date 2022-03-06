using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public struct RectangleF {
        public float X, Y, Width, Height;

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public RectangleF(float x, float y, float w, float h) {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public RectangleF(RectangleI rect) {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        public PointF GetLocation() => new PointF(X, Y);
    }

    public struct RectangleI {
        public int X, Y, Width, Height;

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public RectangleI(int x, int y, int w, int h) {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public RectangleI(RectangleF rect) {
            X = (int)Math.Round(rect.X);
            Y = (int)Math.Round(rect.Y);
            Width = (int)Math.Round(rect.Width);
            Height = (int)Math.Round(rect.Height);
        }

        public PointI GetLocation() => new PointI(X, Y);
    }
}
