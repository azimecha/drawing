using Azimecha.Drawing.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public class IndexEnumerationAdaptor<T> : IEnumerable<T> {
        private IArrayLike<T> _arr;

        public IndexEnumerationAdaptor(IArrayLike<T> arr) { _arr = arr; }

        public IEnumerator<T> GetEnumerator() => new ArrayLikeEnumerator<T>(_arr);
        IEnumerator IEnumerable.GetEnumerator() => new ArrayLikeEnumerator<T>(_arr);
    }

    internal class ArrayLikeEnumerator<T> : IEnumerator<T> {
        private IArrayLike<T> _arr;
        private int _nIndex = -1;

        public ArrayLikeEnumerator(IArrayLike<T> arr) { _arr = arr; }

        public T Current => _arr[_nIndex];
        object IEnumerator.Current => _arr[_nIndex];

        public void Dispose() {
            _arr = null;
        }

        public bool MoveNext() {
            _nIndex++;
            return _nIndex < _arr.Count;
        }

        public void Reset() {
            _nIndex = -1;
        }
    }
}
