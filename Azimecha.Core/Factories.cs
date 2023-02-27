using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public static class Factories<T> {
        public delegate T FactoryDelegate();

        public static T DefaultFactory()
            => (T)Activator.CreateInstance(typeof(T), nonPublic: true);

        public static T ParameterizedFactory(params object[] arrArgs)
            => (T)Activator.CreateInstance(typeof(T), bindingAttr: System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance, binder: null, args: arrArgs, culture: null);
    }
}
