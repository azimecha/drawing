using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public interface IFont {
        float Height { get; }
        PointF GetTextSize(string strText);
    }

    public enum Alignment {
        Near,
        Center,
        Far
    }

    public enum TextWrapping {
        None,
        WordWrap,
        CharWrap
    }

    public struct FontBitmapGlyph {
        public short Width;
        public byte[] Data;
    }
}
