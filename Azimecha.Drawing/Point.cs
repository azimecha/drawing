using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public struct PointF {
        public float X, Y;

        public PointF(float x, float y) {
            X = x;
            Y = y;
        }

        public PointF(PointI pt) {
            X = pt.X;
            Y = pt.Y;
        }
    }

    public struct PointI {
        public int X, Y;

        public PointI(int x, int y) {
            X = x;
            Y = y;
        }

        public PointI(PointF pt) {
            X = (int)Math.Round(pt.X);
            Y = (int)Math.Round(pt.Y);
        }
    }
}
