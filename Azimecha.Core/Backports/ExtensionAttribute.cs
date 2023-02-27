using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute { }
}
