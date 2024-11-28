using System.Collections.Generic;

namespace CGT.Utils
{
    public static class IListExtensions
    {
        public static void AddRange<T>(this IList<T> toAddTo, IList<T> whatToAdd)
        {
            for (int i = 0; i < whatToAdd.Count; i++)
            {
                toAddTo.Add(whatToAdd[i]);
            }
        }

        public static void RemoveAllIn<T>(this IList<T> toRemoveFrom, IList<T> whatToRemove)
        {
            foreach (T item in whatToRemove)
            {
                toRemoveFrom.Remove(item);
            }
        }

        public static IList<T> ReversedCopy<T>(this IList<T> baseList)
        {
            IList<T> result = new List<T>();

            for (int i = baseList.Count - 1; i >= 0; i--)
            {
                result.Add(baseList[i]);
            }

            return result;
        }

        public static void Clear<T>(this IList<T> baseList) where T: class
        {
            for (int i = 0; i < baseList.Count; i++)
            {
                baseList[i] = null;
            }
        }
    }
}