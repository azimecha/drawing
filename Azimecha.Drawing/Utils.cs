using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public static class Utils {
        public static T[] ConvertToArray<T>(IEnumerable<T> enuObjects) {
            List<T> lst = new List<T>();
            foreach (T obj in enuObjects)
                lst.Add(obj);
            return lst.ToArray();
        }
    }

    public class Transformer<TI, TO> : IEnumerable<TO> {
        private TransformProc _proc;
        private IEnumerable<TI> _enuInner;

        public Transformer(IEnumerable<TI> enuInner, TransformProc proc) {
            _enuInner = enuInner;
            _proc = proc;
        }

        public delegate TO TransformProc(TI obj);

        public IEnumerator<TO> GetEnumerator() => new Enumerator(_enuInner.GetEnumerator(), _proc);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class Enumerator : IEnumerator<TO> {
            private TransformProc _proc;
            private IEnumerator<TI> _enuInner;

            public Enumerator(IEnumerator<TI> enuInner, TransformProc proc) {
                _enuInner = enuInner;
                _proc = proc;
            }

            public TO Current => _proc(_enuInner.Current);
            object IEnumerator.Current => Current;

            public void Dispose() => _enuInner.Dispose();
            public bool MoveNext() => _enuInner.MoveNext();
            public void Reset() => _enuInner.Reset();
        }
    }
}
