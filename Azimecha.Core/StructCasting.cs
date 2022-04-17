using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Azimecha.Core {
    public static class StructCasting {
        public static unsafe byte[] MarshalToArray<T>(T objStruct)
            => MarshalToArray((object)objStruct);

        public static unsafe byte[] MarshalToArray(object objStruct) {
            byte[] arrData = new byte[Marshal.SizeOf(objStruct)];

            fixed (byte* pData = arrData)
                Marshal.StructureToPtr(objStruct, (IntPtr)pData, false);

            return arrData;
        }

        public static unsafe T MarshalFromArray<T>(byte[] arrData, int nOffset = 0)
            => (T)MarshalFromArray(typeof(T), arrData, nOffset);

        public static unsafe object MarshalFromArray(Type typeStruct, byte[] arrData, int nOffset = 0) {
            fixed (byte* pData = arrData)
                return Marshal.PtrToStructure((IntPtr)(pData + nOffset), typeStruct);
        }

        public static TBase Cast<TDerived, TBase>(TDerived obj)
            => (TBase)Cast(obj, typeof(TBase));

        public static object Cast(object obj, Type typeBase) {
            object objResult = TryCast(obj, typeBase);
            if (objResult is null)
                throw new InvalidCastException($"Type {obj.GetType().FullName} does not have {typeBase.FullName} as a base struct type");
            return objResult;
        }

        public static TBase? TryCast<TDerived, TBase>(TDerived obj) where TBase : struct
            => Cast(obj, typeof(TBase)) as TBase?;

        public static object TryCast(object obj, Type typeBase) {
            Type typeDerived = obj.GetType();

            if (typeDerived == typeBase)
                return obj;

            BaseStructAttribute attribBaseStruct = GetBSA(typeDerived, typeBase);
            if (!(attribBaseStruct is null))
                return attribBaseStruct.GetValue(obj);

            /*foreach (FieldInfo infField in typeDerived.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)) {
                if (infField.FieldType == typeBase)
                    foreach (BaseStructMemberAttribute attrib in infField.GetCustomAttributes(true))
                        return infField.GetValue(obj);
            }*/

            return null;
        }

        public static bool CanCast(Type typeDerived, Type typeBase)
            => (typeDerived == typeBase) || !(GetBSA(typeDerived, typeBase) is null);

        private static BaseStructAttribute GetBSA(Type typeDerived, Type typeBase) {
            foreach (BaseStructAttribute attrib in GetBSAs(typeDerived)) {
                if (attrib.BaseType == typeBase)
                    return attrib;
            }

            return null;
        }

        private static IEnumerable<BaseStructAttribute> GetBSAs(Type typeDerived) {
            List<BaseStructAttribute> lstBSAs = new List<BaseStructAttribute>();

            foreach (Attribute attrib in typeDerived.GetCustomAttributes(true)) {
                if (attrib is BaseStructAttribute attribBSA) {
                    lstBSAs.Add(attribBSA);
                    lstBSAs.AddRange(GetBSAs(attribBSA.BaseType));
                }
            }

            return lstBSAs;
        }
    }

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
    public class BaseStructAttribute : Attribute {
        public BaseStructAttribute(Type typeBase) {
            BaseType = typeBase;
            Offset = 0;
        }

        public BaseStructAttribute(Type typeBase, int nOffset) {
            BaseType = typeBase;
            Offset = nOffset;
        }

        public Type BaseType { get; set; }
        public int Offset { get; set; }

        public object GetValue(object objStruct) {
            byte[] arrData = StructCasting.MarshalToArray(objStruct);
            return StructCasting.MarshalFromArray(BaseType, arrData, Offset);
        }
    }

    //[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    //public class BaseStructMemberAttribute : Attribute { }
}
