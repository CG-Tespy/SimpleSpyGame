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
    }
}