using System;
using System.Collections.Generic;

namespace IGS.Unity
{
    public static class RandomExtensions
    {
        #region Array
        public static T GetRandomItem<T>(this T[] inArray)
        {
            return inArray[UnityEngine.Random.Range(0, inArray.Length)];
        }

        public static void Shuffle<T>(this T[] inArray)
        {
            for(int i=0; i<inArray.Length-1; i++)
            {
                int pos = UnityEngine.Random.Range(i, inArray.Length);
                T temp = inArray[i];
                inArray[i] = inArray[pos];
                inArray[pos] = temp;    
            }
        }
        #endregion

        #region List
        public static T GetRandomItem<T>(this IList<T> inList)
        {
            return inList[UnityEngine.Random.Range(0, inList.Count)];   
        }

        public static void Shuffle<T>(this IList<T> inList)
        {
            for(int i=0; i<inList.Count-1; i++)
            {
                int pos = UnityEngine.Random.Range(i, inList.Count);
                T temp = inList[i];
                inList[i] = inList[pos];
                inList[pos] = temp;
            }
        }
        #endregion
    }
}
