using System;

namespace IGS.Unity
{
    public static class EqualityExtensions
    {
        public static bool IsNullable<T>(this T obj)
        {
            if(ReferenceEquals(obj, null))
                return true;

            Type type = typeof(T);

            if(!type.IsValueType)
                return true; // ref-type

            if(Nullable.GetUnderlyingType(type) != null)
                return true; // Nullable<T>

            return false; // value-type
        }

        public static bool IsNull<T>(this T obj)
        {
            if(!IsNullable(obj))
                return false;

            return ReferenceEquals(obj, null);
        }

        public static bool SafeEquals(this object obj, object other)
        {
            if(IsNull(obj) && IsNull(other))
                return true; // both nulls

            return ReferenceEquals(obj, other);
        }
    }
}
