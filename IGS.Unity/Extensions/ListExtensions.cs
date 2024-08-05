using System;
using System.Collections.Generic;

namespace IGS.Unity
{
    public static class ListExtensions
    {
        public static T First<T>(this IList<T> inList)
        {
            return inList[0];
        }

        public static T Last<T>(this IList<T> inList)
        {
            return inList[inList.Count - 1];
        }
           
        public static List<T> Copy<T>(this IList<T> inList)
        {
            List<T> outList = new List<T>();

            for(int i=0; i<inList.Count; i++)
            {
                outList.Add(inList[i]);
            }

            return outList;
        }

        public static void Reverse<T>(this IList<T> inList)
        {
            int left = 0;
            int right = inList.Count - 1;

            while(left < right)
            {
                T temp = inList[left];
                inList[left] = inList[right];
                inList[right] = temp;

                left++;
                right--;
            }
        }

        public static bool AddUnique<T>(this IList<T> inList, T item)
        {
            if(item.IsNull())
                return false;

            if(inList.Contains(item))
                return false;

            inList.Add(item);

            return true;
        }

        public static void Distinct<T>(this IList<T> inList)
        {
            for(int i=inList.Count-1; i>=0; i--)
            {
                if(inList[i].IsNull())
                {
                    inList.RemoveAt(i);
                }
                else
                {
                    for(int j=inList.Count-1; j>i; j--)
                    {
                        if(inList[i].SafeEquals(inList[j]))
                        {
                            inList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public static bool ContainsNull<T>(this IList<T> inList)
        {
            foreach(var item in inList)
            {
                if(item.IsNull())
                    return true;
            }

            return false;
        }

        public static int RemoveNulls<T>(this IList<T> inList)
        {
            int cnt = 0;

            for(int i=inList.Count-1; i>=0; i--)
            {
                if(inList[i].IsNull())
                {
                    ++cnt;
                    inList.RemoveAt(i);
                }
            }

            return cnt;
        }

        public static List<T> Where<T>(this IList<T> inList, Predicate<T> predicate)
        {
            List<T> outList = new List<T>();

            foreach(var item in inList)
            {
                if(predicate(item))
                {
                    outList.Add(item);  
                }
            }

            return outList;
        }

        public static void ForEach<T>(this IList<T> inList, Action<T> action)
        {
            foreach(var item in inList)
            {
                action(item);
            }
        }

        public static int ForEachWhere<T>(this IList<T> inList, Action<T> action, Predicate<T> predicate)
        {
            int cnt = 0;

            foreach(var item in inList)
            {
                if(predicate(item))
                {
                    ++cnt;
                    action(item);
                }
            }

            return cnt;
        }
        // A & B
        public static List<T> Intersection<T>(this IList<T> aList, IList<T> bList)
        {
            List<T> outList = new List<T>();

            aList.ForEachWhere(
                a => outList.Add(a),
                a => bList.Contains(a));

            return outList;
        }
        // A | B
        public static List<T> Union<T>(this IList<T> aList, IList<T> bList)
        {
            List<T> outList = new List<T>();

            aList.ForEach(a => outList.Add(a));
            bList.ForEach(b => outList.Add(b)); 

            return outList;
        }
        // A - B
        public static List<T> Difference<T>(this IList<T> aList, IList<T> bList)
        {
            List<T> outList = new List<T>();

            aList.ForEachWhere(
                a => outList.Add(a),
                a => !bList.Contains(a));

            return outList;
        }
        // A ^ B = (A | B) - (A & B)
        public static List<T> SymDifference<T>(this IList<T> aList, IList<T> bList)
        {
            List<T> unionList = aList.Union(bList);
            List<T> intersectList = aList.Intersection(bList);

            return unionList.Difference(intersectList);
        }

        public static List<string> ToStringList<T>(this IList<T> inList)
        {
            List<string> outList = new List<string>();

            foreach (var item in inList)
            {
                outList.Add(item.IsNull() ? "NULL" : item.ToString());
            }

            return outList;
        }
    }
}
