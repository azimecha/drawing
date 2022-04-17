using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public class Weak<T> : IEquatable<Weak<T>>, IEquatable<T> where T : class {
        private WeakReference _ref;

        public Weak(T obj) {
            _ref = new WeakReference(obj);
        }

        public T Target => (T)_ref.Target;

        public bool Equals(Weak<T> other)
            => ReferenceEquals(_ref.Target, other.Target);

        public bool Equals(T other)
            => ReferenceEquals(_ref.Target, other);

        public override bool Equals(object obj) {
            if (obj is Weak<T> refOther)
                return Equals(refOther);
            else if (obj is T objOther)
                return Equals(objOther);
            else
                return false;
        }

        public override int GetHashCode()
            => _ref.Target?.GetHashCode() ?? 0;
    }
}
