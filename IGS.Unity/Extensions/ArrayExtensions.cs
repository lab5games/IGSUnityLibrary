using System;
using System.Collections.Generic;

namespace IGS.Unity
{
    public static class ArrayExtensions
    {
        public static T First<T>(this T[] inArray)
        {
            return inArray[0];
        }

        public static T Last<T>(this T[] inArray)
        {
            return inArray[inArray.Length - 1]; 
        }

        public static T[] Copy<T>(this T[] inArray)
        {
            T[] outArray = new T[inArray.Length];

            for(int i=0; i<outArray.Length; i++)
                outArray[i] = inArray[i];

            return outArray;
        }

        public static void Reverse<T>(this T[] inArray)
        {
            int left = 0;
            int right = inArray.Length - 1;

            while(left < right)
            {
                T temp = inArray[left];
                inArray[left] = inArray[right];
                inArray[right] = temp;

                left++;
                right--;
            }
        }

        public static bool ContainsNull<T>(this T[] inArray)
        {
            for(int i=0; i<inArray.Length; i++)
            {
                if(inArray[i].IsNull())
                    return true;
            }

            return false;
        }

        public static List<T> Where<T>(this T[] inArray, Predicate<T> predicate)
        {
            List<T> outList = new List<T>();

            for(int i = 0; i < inArray.Length; i++)
            {
                if(predicate(inArray[i]))
                {
                    outList.Add(inArray[i]);
                }
            }

            return outList;
        }

        public static void ForEach<T>(this T[] inArray, Action<T> action)
        {
            for(int i=0; i<inArray.Length; i++)
            {
                action(inArray[i]);
            }
        }

        public static int ForEachWhere<T>(this T[] inArray, Action<T> action, Predicate<T> predicate)
        {
            int cnt = 0;

            for(int i=0; i<inArray.Length; i++)
            {
                if(predicate(inArray[i]))
                {
                    ++cnt;
                    action(inArray[i]);
                }
            }

            return cnt;
        }


        public static int Partition<T>(this T[] inArray, Predicate<T> predicate)
        {
            int left = 0;
            int right = inArray.Length - 1;

            while(left < right)
            {
                while(left < right && predicate(inArray[left]))
                {
                    left++;
                }

                while(left < right && !predicate(inArray[right]))
                {
                    right--;
                }

                if(left < right)
                {
                    T temp = inArray[left];
                    inArray[left] = inArray[right];
                    inArray[right] = temp;
                }
            }

            return left;
        }

        public static List<string> ToStringList<T>(this T[] inArray)
        {
            List<string> outList = new List<string>();

            for(int i=0; i<inArray.Length; i++)
            {
                outList.Add(inArray[i].IsNull() ? "NULL" : inArray[i].ToString());
            }

            return outList;
        }
    }
}
