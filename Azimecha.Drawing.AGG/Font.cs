using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public abstract class Font : IFont {
        private SafeFontHandle _hFont = new SafeFontHandle();

        internal Interop.AwFontInfo FontInfo {
            get {
                if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwGetFontInfo>()(_hFont.Handle, out Interop.AwFontInfo infFont))
                    throw new InfoQueryFailedException($"Error querying information for font {_hFont}");
                return infFont;
            }
        }

        public float Height => FontInfo.nHeight;

        public PointF GetTextSize(string strText) {
            byte[] arrData = StringToBytes(strText);

            PointF ptSize;
            if (!Interop.Functions.Loader.GetMethod<Interop.Functions.AwGetTextSize>()(_hFont.Handle, arrData, out ptSize.X, out ptSize.Y))
                throw new InfoQueryFailedException($"Error calculating size of text \"{strText}\" with font {_hFont}");

            return ptSize;
        }

        protected internal static byte[] StringToBytes(string str)
            => Encoding.UTF8.GetBytes(str + '\0');

        internal SafeFontHandle Handle => _hFont;
    }

    internal class SafeFontHandle : Internal.SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeleteFont>()(hObject);
        }
    }
}
