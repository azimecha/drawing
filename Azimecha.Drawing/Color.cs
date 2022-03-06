using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public struct Color {
        public byte R, G, B, A;

        public Color(byte r, byte g, byte b, byte a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
