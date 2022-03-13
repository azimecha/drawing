using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public static class LibraryLoader {
        public static INativeLibrary Load(string strName) {
#if NETFRAMEWORK
            return new WindowsDLL(strName);
#else
            return new DotnetNativeLibrary(strName);
#endif
        }

        public static string FileExtension {
            get {
                switch (Environment.OSVersion.Platform) {
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                    case PlatformID.WinCE:
                    case PlatformID.Xbox:
                        return "dll";

                    case PlatformID.Unix:
                    default: // other weird platforms will probably be unix like
                        return "so";

                    case PlatformID.MacOSX:
                        return "dylib";
                }
            }
        }
    }

    public interface INativeLibrary : IDisposable {
        IntPtr GetSymbolAddress(string strName);
        IntPtr? TryGetSymbolAddress(string strName);
    }

#if NETFRAMEWORK
    internal class WindowsDLL : INativeLibrary {
        private SafeWindowsModuleHandle _hModule = new SafeWindowsModuleHandle();
        private string _strName = "";

        public WindowsDLL(string strName) {
            _hModule.TakeObject(LoadLibrary(strName), true);
            if (!_hModule.IsHandleValid)
                throw new Win32Exception();

            _strName = strName;
        }

        public IntPtr GetSymbolAddress(string strName) {
            IntPtr pSym = GetProcAddress(_hModule.Handle, strName);

            if (pSym == IntPtr.Zero)
                throw new EntryPointNotFoundException($"The entry point {strName} was not found in {_strName}");

            return pSym;
        }

        public IntPtr? TryGetSymbolAddress(string strName) {
            IntPtr pSym = GetProcAddress(_hModule.Handle, strName);
            return (pSym == IntPtr.Zero) ? (IntPtr?)null : pSym;
        }

        public void Dispose() {
            _hModule?.Dispose();
        }

        public override string ToString() {
            return $"{_strName} @ {_hModule.Handle}";
        }

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string strName);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hLibrary, string strName);
    }

    internal class SafeWindowsModuleHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            FreeLibrary(hObject);
        }

        [DllImport("kernel32")]
        public static extern bool FreeLibrary(IntPtr hLibrary);
    }
#else
    internal class DotnetNativeLibrary : INativeLibrary {
        private SafeDotnetNativeLibraryHandle _hLibrary = new SafeDotnetNativeLibraryHandle();
        private string _strName = "";

        public DotnetNativeLibrary(string strName) {
            _hLibrary.TakeObject(NativeLibrary.Load(strName), true);
            _strName = strName;
        }

        public IntPtr GetSymbolAddress(string strName)
            => NativeLibrary.GetExport(_hLibrary.Handle, strName);

        public IntPtr? TryGetSymbolAddress(string strName)
            => NativeLibrary.TryGetExport(_hLibrary.Handle, strName, out IntPtr pSym) ? (IntPtr?)pSym : null;

        public void Dispose() {
            _hLibrary?.Dispose();
        }

        public override string ToString() {
            return $"{_strName} @ {_hLibrary.Handle}";
        }
    }

    internal class SafeDotnetNativeLibraryHandle : SafeHandle {
        protected override void CloseObjectHandle(IntPtr hObject) {
            NativeLibrary.Free(hObject);
        }
    }
#endif
}
