using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Core {
    public class Union {
        private int _nSize = 0;
        private Dictionary<string, Type> _dicMembers = new Dictionary<string, Type>();

        public int Size => _nSize;

        public void SetMinimumSize(int nSize) {
            _nSize = nSize;
        }

        public void AddMember<T>() 
            => AddMember(typeof(T));

        public void AddMember<T>(string strName) 
            => AddMember(typeof(T), strName);

        public void AddMember(Type type) 
            => AddMember(type, type.Name);

        public void AddMember(Type type, string strName) {
            int nSize = Marshal.SizeOf(type);
            if (nSize > _nSize)
                _nSize = nSize;

            _dicMembers.Add(strName, type);
        }

        public object GetValue(IntPtr pUnion, string strMembName)
            => Marshal.PtrToStructure(pUnion, _dicMembers[strMembName]);

        public object GetValue(IntPtr pUnion, Type type)
            => GetValue(pUnion, type.Name);

        public T GetValue<T>(IntPtr pUnion, string strMembName)
            => (T)GetValue(pUnion, strMembName);

        public T GetValue<T>(IntPtr pUnion)
            => GetValue<T>(pUnion, typeof(T).Name);

        public unsafe object GetValue(byte[] arrUnion, string strMembName) {
            if (arrUnion.Length < _nSize)
                throw new ArgumentException($"Array size {arrUnion.Length} is below union size {_nSize}");

            fixed (byte* pUnion = arrUnion)
                return GetValue((IntPtr)pUnion, strMembName);
        }

        public object GetValue(byte[] arrUnion, Type type)
            => GetValue(arrUnion, type.Name);

        public T GetValue<T>(byte[] arrUnion, string strMembName)
            => (T)GetValue(arrUnion, strMembName);

        public T GetValue<T>(byte[] arrUnion)
            => GetValue<T>(arrUnion, typeof(T).Name);
    }
}
