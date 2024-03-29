﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Core {
    public static class Utils {
        public static T[] ConvertToArray<T>(IEnumerable<T> enuObjects) {
            List<T> lst = new List<T>();
            foreach (T obj in enuObjects)
                lst.Add(obj);
            return lst.ToArray();
        }

        public static T TryGetAttribute<T>(Type type) where T : Attribute {
            foreach (Attribute attrib in type.GetCustomAttributes(true))
                if (attrib is T)
                    return (T)attrib;
            return null;
        }

        public static byte ReverseBits(byte nValue) => REVERSED_BYTES[nValue];

        private static readonly byte[] REVERSED_BYTES = {
            0x00, 0x80, 0x40, 0xc0, 0x20, 0xa0, 0x60, 0xe0,
            0x10, 0x90, 0x50, 0xd0, 0x30, 0xb0, 0x70, 0xf0,
            0x08, 0x88, 0x48, 0xc8, 0x28, 0xa8, 0x68, 0xe8,
            0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 0x78, 0xf8,
            0x04, 0x84, 0x44, 0xc4, 0x24, 0xa4, 0x64, 0xe4,
            0x14, 0x94, 0x54, 0xd4, 0x34, 0xb4, 0x74, 0xf4,
            0x0c, 0x8c, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec,
            0x1c, 0x9c, 0x5c, 0xdc, 0x3c, 0xbc, 0x7c, 0xfc,
            0x02, 0x82, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2,
            0x12, 0x92, 0x52, 0xd2, 0x32, 0xb2, 0x72, 0xf2,
            0x0a, 0x8a, 0x4a, 0xca, 0x2a, 0xaa, 0x6a, 0xea,
            0x1a, 0x9a, 0x5a, 0xda, 0x3a, 0xba, 0x7a, 0xfa,
            0x06, 0x86, 0x46, 0xc6, 0x26, 0xa6, 0x66, 0xe6,
            0x16, 0x96, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6,
            0x0e, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 0x6e, 0xee,
            0x1e, 0x9e, 0x5e, 0xde, 0x3e, 0xbe, 0x7e, 0xfe,
            0x01, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1,
            0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1,
            0x09, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9,
            0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9,
            0x05, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5,
            0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5,
            0x0d, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed,
            0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd,
            0x03, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3,
            0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3,
            0x0b, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb,
            0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb,
            0x07, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7,
            0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7,
            0x0f, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef,
            0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff
        };

        public static string ConvertLFToCRLF(string strLF) {
            string strCRLF = string.Empty;

            for (int nChar = 0; nChar < strLF.Length; nChar++) {
                if (strLF[nChar] == '\n' && (nChar == 0 || strLF[nChar - 1] != '\r'))
                    strCRLF += "\r\n";
                else
                    strCRLF += strLF[nChar];
            }

            return strCRLF;
        }

        public static unsafe byte[] PtrToNarrowChars(IntPtr pData) {
            byte* pszData = (byte*)pData;

            int nLength = 0;
            while (pszData[nLength] != 0)
                nLength++;

            byte[] arrData = new byte[nLength];
            for (int i = 0; i < nLength; i++)
                arrData[i] = pszData[i];

            return arrData;
        }

        public static string PtrToStringUTF8(IntPtr pData) => Encoding.UTF8.GetString(PtrToNarrowChars(pData));

        public static byte[] StringToUTF8Z(string str) => Encoding.UTF8.GetBytes(str + "\0");

        public static string ArrayToStringUTF8Z(byte[] arrData) {
            byte[] arrBeforeNull = null;

            for (int i = 0; i < arrData.Length; i++) {
                if (arrData[i] == 0) {
                    arrBeforeNull = new byte[i];
                    Array.Copy(arrData, arrBeforeNull, i);
                    break;
                }
            }

            if (arrBeforeNull is null)
                arrBeforeNull = arrData;

            return Encoding.UTF8.GetString(arrBeforeNull);
        }

        public static ulong IntPtrMaxValue {
            get {
                if (IntPtr.Size >= 8)
                    return ulong.MaxValue;

                return (1UL << IntPtr.Size * 8) - 1;
            }
        }

        private const int READ_BLOCK_SIZE = 4096;

        public static long ReadStreamToPointer(System.IO.Stream stmRead, IntPtr pBuffer, long nMaxBytes) {
            byte[] arrTemp = new byte[READ_BLOCK_SIZE];
            long nTotalBytesRead = 0;

            while (nTotalBytesRead < nMaxBytes) {
                long nBytesLeft = nMaxBytes - nTotalBytesRead;

                int nToRead = READ_BLOCK_SIZE;
                if (nToRead > nBytesLeft)
                    nToRead = (int)nBytesLeft;

                int nNewBytesRead = stmRead.Read(arrTemp, 0, nToRead);
                if (nNewBytesRead <= 0)
                    break;

                Marshal.Copy(arrTemp, 0, (IntPtr)((long)pBuffer + nTotalBytesRead), nNewBytesRead);

                nTotalBytesRead += nNewBytesRead;
            }

            return nTotalBytesRead;
        }

        [DllImport("kernel32")]
        private static extern void RtlMoveMemory(IntPtr pDest, IntPtr pSource, IntPtr nBytes);

        public static void CopyMemory(IntPtr pSource, IntPtr pDest, ulong nBytes) {
            RtlMoveMemory(pDest, pSource, (IntPtr)nBytes);
        }

        public static T TryGetAttribute<T>(System.Reflection.ICustomAttributeProvider provider) where T : Attribute
            => (T)TryGetAttribute(provider, typeof(T));

        public static Attribute TryGetAttribute(System.Reflection.ICustomAttributeProvider provider, Type typeAttrib) {
            foreach (Attribute attrib in provider.GetCustomAttributes(true))
                if (typeAttrib.IsAssignableFrom(attrib.GetType()))
                    return attrib;
            return null;
        }

        public static object InvokeAndUnwrap(this MethodBase meth, object objTarget, params object[] arrArgs) {
            try {
                return meth.Invoke(objTarget, arrArgs);
            } catch (TargetInvocationException ex) {
                throw ex.InnerException;
            }
        }

        public static void DoAll(params Subroutine[] arrToDo)
            => DoAll((IEnumerable<Subroutine>)arrToDo);

        public static void DoAll(IEnumerable<Subroutine> enuToDo) {
            List<Exception> lstExceptions = null;

            foreach (Subroutine act in enuToDo) {
                try {
                    act();
                } catch (Exception ex) {
                    if (lstExceptions is null)
                        lstExceptions = new List<Exception>();
                    lstExceptions.Add(ex);
                }
            }

            MultipleErrorException.MaybeThrow(lstExceptions);
        }

        public static T[] ToArray<T>(this IEnumerable<T> enuObjects)
            => new List<T>(enuObjects).ToArray();

        public static object[] ToObjectArray(this IEnumerable enuObjects) {
            List<object> lstObjects = new List<object>();
            foreach (object obj in enuObjects)
                lstObjects.Add(obj);
            return lstObjects.ToArray();
        }

        public static IEnumerable<TOut> TransformObjects<TOut>(this IEnumerable enuObjects,
            TransformerDelegate<object, TOut> procTransformer)
        {
            foreach (object objIn in enuObjects)
                yield return procTransformer(objIn);
        }

        public static IEnumerable<TOut> Transform<TIn, TOut>(this IEnumerable<TIn> enuObjects, 
            TransformerDelegate<TIn, TOut> procTransformer)
        {
            foreach (TIn valIn in enuObjects)
                yield return procTransformer(valIn);
        }

        public static string Join(this IEnumerable enuObjects, string strSeparator)
            => string.Join(strSeparator, enuObjects.TransformObjects(obj => obj.ToString()).ToArray());

        public static bool IsEmpty(this IEnumerable enuObjects) {
            foreach (object obj in enuObjects)
                return false;
            return true;
        }

        public static int GetCount(this IEnumerable enuObjects) {
            int nCount = 0;
            foreach (object obj in enuObjects)
                nCount++;
            return nCount;
        }

        public static T GetItem<T>(this IEnumerable<T> enuObjects, int nDesiredIndex) {
            int nCurIndex = 0;

            foreach (T val in enuObjects) {
                if (nCurIndex == nDesiredIndex)
                    return val;
                else
                    nCurIndex++;
            }

            throw new IndexOutOfRangeException($"The index {nDesiredIndex} was beyond the end "
                + $"of the {nCurIndex} item enumerable");
        }

        public static IEnumerable<TValue> Flatten<TValue, TEnumerable>(this IEnumerable<TEnumerable> enuOfEnumerables)
            where TEnumerable : IEnumerable<TValue>    
        {
            foreach (TEnumerable enuCur in enuOfEnumerables)
                foreach (TValue val in enuCur)
                    yield return val;
        }

        public static IEnumerable<T> GetInstancesOf<T>(this IEnumerable enuObjects) {
            foreach (object obj in enuObjects)
                if (obj is T val)
                    yield return val;
        }

        public static TOut[] AttemptTransformAll<TIn, TOut>(this IEnumerable<TIn> enuObjects, 
            TransformerDelegate<TIn, TOut> procTransform)
        {
            List<TOut> lstResults = new List<TOut>();
            List<Exception> lstExceptions = null;

            foreach (TIn val in enuObjects) {
                try {
                    lstResults.Add(procTransform(val));
                } catch (Exception ex) {
                    if (lstExceptions is null)
                        lstExceptions = new List<Exception>();
                    lstExceptions.Add(ex);
                }
            }

            MultipleErrorException.MaybeThrow(lstExceptions);
            return lstResults.ToArray();
        }

        public static void AttemptOnAll<T>(this IEnumerable<T> enuObjects,
            Subroutine<T> procAction)
        {
            List<Exception> lstExceptions = null;

            foreach (T val in enuObjects) {
                try {
                    procAction(val);
                } catch (Exception ex) {
                    if (lstExceptions is null)
                        lstExceptions = new List<Exception>();
                    lstExceptions.Add(ex);
                }
            }

            MultipleErrorException.MaybeThrow(lstExceptions);
        }

        public static T LastElementOrDefault<T>(this T[] arrObjects)
            => (arrObjects.Length > 0) ? arrObjects[arrObjects.Length - 1] : default(T);

        public static bool TryGetFirstMatch<T>(this IEnumerable<T> enuObjects, FilteringDelegate<T> procFilter, 
            out T valIfFound)
        {
            foreach (T valCur in enuObjects) {
                if (procFilter(valCur)) {
                    valIfFound = valCur;
                    return true;
                }
            }

            valIfFound = default(T);
            return false;
        }

        public static T GetFirstOr<T>(this IEnumerable<T> enuObjects, T valDefault = default(T)) {
            foreach (T valCur in enuObjects)
                return valCur;
            return valDefault;
        }

        public static bool ContainsReference<T>(this IEnumerable<T> enuObjects, T valSearchFor) where T : class {
            foreach (T valCur in enuObjects)
                if (ReferenceEquals(valCur, valSearchFor))
                    return true;
            return false;
        }

        public static D ToDelegate<D>(this MethodInfo infMethod) where D : Delegate
            => (D)Delegate.CreateDelegate(typeof(D), infMethod, true);

        public static D ToDelegate<D>(this MethodInfo infMethod, object objBoundArg) where D : Delegate
            => (D)Delegate.CreateDelegate(typeof(D), objBoundArg, infMethod, true);

        public static TMember GetMemberByName<TMember>(this Type typeContaining, string strName) where TMember : MemberInfo {
            TMember infMemb = typeContaining.GetMember(strName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public 
                | BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy).GetInstancesOf<TMember>().GetFirstOr(null);

            if (infMemb is null)
                throw new MissingMemberException($"Could not find {strName} in {typeContaining.FullName}");

            return infMemb;
        }

        public static unsafe void Clear(this IDataBuffer buf) {
            ulong* pBufAsUlong = (ulong*)buf.DataPointer;
            for (long nCurrentUlong = 0; nCurrentUlong < buf.DataSize / sizeof(ulong); nCurrentUlong++) {
                *pBufAsUlong = 0;
                pBufAsUlong++;
            }

            byte* pRemainingData = (byte*)pBufAsUlong;
            for (int nCurrentByte = 0; nCurrentByte < buf.DataSize % sizeof(ulong); nCurrentByte++) {
                *pRemainingData = 0;
                pRemainingData++;
            }
        }
    }

    public delegate void Subroutine();
    public delegate void Subroutine<T1>(T1 val1);
    public delegate void Subroutine<T1, T2>(T1 val1, T2 val2);
    public delegate void Subroutine<T1, T2, T3>(T1 val1, T2 val2, T3 val3);
    public delegate TOut TransformerDelegate<TIn, TOut>(TIn val);
    public delegate bool FilteringDelegate<TIn>(TIn val);
}

namespace Azimecha.Drawing.Internal {
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
