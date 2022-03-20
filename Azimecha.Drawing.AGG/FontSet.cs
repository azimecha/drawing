using Azimecha.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public abstract class FontSet : IFontSet {
        private IndexEnumerationAdaptor<IFontPrototype> _adaptor;
        private SafeFontSetHandle _hSet;
        private int _nCount;

        internal FontSet(SafeFontSetHandle hSet) {
            _hSet = hSet;

            _nCount = Interop.Functions.Loader.GetMethod<Interop.Functions.AwGetFontSetSize>()(_hSet.Handle);
            if (_nCount < 0)
                throw new InfoQueryFailedException("Unable to get size of font set");

            _adaptor = new IndexEnumerationAdaptor<IFontPrototype>(this);
        }

        public IFontPrototype this[int nIndex] {
            get {
                if ((nIndex < 0) || (nIndex >= _nCount))
                    throw new IndexOutOfRangeException($"Invalid font index {nIndex} (there are {_nCount} fonts in this set)");
                return new Prototype(this, nIndex);
            }
        }

        public int Count => _nCount;
        public IEnumerator<IFontPrototype> GetEnumerator() => _adaptor.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _adaptor.GetEnumerator();
        internal SafeFontSetHandle Handle => _hSet;

        private class Prototype : IFontPrototype {
            private FontSet _set;
            private int _nIndex;
            private string _strName;

            public Prototype(FontSet set, int nIndex) {
                _set = set;
                _nIndex = nIndex;

                IntPtr pName = Interop.Functions.Loader.GetMethod<Interop.Functions.AwGetFontNameFromSet>()(set._hSet.Handle, nIndex);
                _strName = (pName != IntPtr.Zero) ? Utils.PtrToStringUTF8(pName) : "";
            }

            public string Name => _strName;
            public IFont CreateFont(int nHeightPixels) => new SetFont(_set, _nIndex, nHeightPixels);
            public override string ToString() => _strName ?? "(unset)";
        }
    }

    internal class SetFont : Font {
        public SetFont(FontSet set, int nFont, int nHeightPixels) {
            Handle.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateFontFromSet>()(set.Handle.Handle, nFont, nHeightPixels), true);
            if (!Handle.IsHandleValid)
                throw new ObjectCreationFailedException($"Error creating {nHeightPixels}px instance of font {nFont} in set {set}");
        }
    }

    internal class SafeFontSetHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            Interop.Functions.Loader.GetMethod<Interop.Functions.AwDeleteFontSet>()(hObject);
        }
    }
}
