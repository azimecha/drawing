using Azimecha.Drawing.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Core {
    public class SmartLoader : IDisposable {
        private Type _typeClass;
        private IDictionary<Type, Delegate> _dicMethods = new Dictionary<Type, Delegate>();
        private INativeLibrary _library;

        public SmartLoader(Type typeClass, string strLibraryName) {
            _typeClass = typeClass;
            _library = FindBestLibrary(strLibraryName);

            foreach (Type typeMethod in _typeClass.GetNestedTypes()) {
                if (!typeMethod.IsSubclassOf(typeof(Delegate)))
                    continue;

                SmartImportAttribute attrib = Utils.TryGetAttribute<SmartImportAttribute>(typeMethod);
                if (attrib is null)
                    continue;

                _dicMethods.Add(typeMethod, FindExportedMethod(_library, typeMethod));
            }

            // keep the library loaded permanently - delegates could still be around when this object gets deleted
            _library.MakePermanent();
        }

        public T GetMethod<T>() where T : Delegate
            => (T)_dicMethods[typeof(T)];

        private static INativeLibrary FindBestLibrary(string strName) {
            List<string> lstSuffixToTry = new List<string>();

            switch (SystemInfo.Architecture) {
                case CPUArchitecture.IA32:
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE3))
                        lstSuffixToTry.Add("32_sse3");
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE2))
                        lstSuffixToTry.Add("32_sse2");
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE1))
                        lstSuffixToTry.Add("32_sse");
                    if (SystemInfo.HasCPUFeature(CPUFeature.MMX))
                        lstSuffixToTry.Add("32_mmx");
                    if (!SystemInfo.HasCPUFeature(CPUFeature.NoFPU))
                        lstSuffixToTry.Add("32_486");
                    lstSuffixToTry.Add("32_386"); // can even .NET 1 run on a 386?
                    break;

                case CPUArchitecture.AMD64:
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE3))
                        lstSuffixToTry.Add("64_sse3");
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE2))
                        lstSuffixToTry.Add("64_sse2");
                    if (SystemInfo.HasCPUFeature(CPUFeature.SSE1))
                        lstSuffixToTry.Add("64_sse");
                    break;

                case CPUArchitecture.ARM:
                    lstSuffixToTry.Add("_arm32");
                    break;

                case CPUArchitecture.IA64:
                    lstSuffixToTry.Add("_ia64");
                    break;

                case CPUArchitecture.ARM64:
                    lstSuffixToTry.Add("_arm64");
                    break;
            }

            lstSuffixToTry.Add((IntPtr.Size * 8).ToString());
            lstSuffixToTry.Add(string.Empty);

            foreach (string strSuffix in lstSuffixToTry) {
                string strFullName = $"{strName}{strSuffix}.{LibraryLoader.FileExtension}";
                try {
                    return LibraryLoader.Load(strFullName);
                } catch (Exception) { }
            }

            throw new DllNotFoundException($"Library {strName} could not be loaded (tried {lstSuffixToTry.Count} suffixes)");
        }

        private static Delegate FindExportedMethod(INativeLibrary library, Type typeMethod) {
            List<string> lstToTry = new List<string>();
            MethodInfo infMethod = typeMethod.GetMethod("Invoke");

            lstToTry.Add(typeMethod.Name);

            UnmanagedFunctionPointerAttribute attribFuncPtr = Utils.TryGetAttribute<UnmanagedFunctionPointerAttribute>(typeMethod);
            if (attribFuncPtr is null || attribFuncPtr.CallingConvention == CallingConvention.StdCall)
                lstToTry.Add("_" + typeMethod.Name + "@" + GetTotalParamsSize(infMethod));
            else if (attribFuncPtr.CallingConvention == CallingConvention.Cdecl)
                lstToTry.Add("_" + typeMethod.Name);

            IntPtr? pFunc = null;
            foreach (string strToTry in lstToTry) {
                pFunc = library.TryGetSymbolAddress(strToTry);
                if (!(pFunc is null)) break;
            }

            if (pFunc is null)
                throw new EntryPointNotFoundException($"The function {typeMethod.Name} could not be found in {library}");

            return Marshal.GetDelegateForFunctionPointer(pFunc.Value, typeMethod);
        }

        private static int GetTotalParamsSize(MethodInfo infMethod) {
            int nTotalSize = 0;

            foreach (ParameterInfo infParam in infMethod.GetParameters()) {
                int nRawSize = 0;

                if (infParam.ParameterType.IsPrimitive)
                    nRawSize = Marshal.SizeOf(infParam.ParameterType);

                nTotalSize += nRawSize < IntPtr.Size ? IntPtr.Size : nRawSize;
            }

            return nTotalSize;
        }

        public void Dispose() {
            _dicMethods.Clear();
            _typeClass = null;
        }
    }
}

namespace Azimecha.Drawing.Internal {
    [AttributeUsage(AttributeTargets.Delegate)]
    public class SmartImportAttribute : Attribute { }
}
