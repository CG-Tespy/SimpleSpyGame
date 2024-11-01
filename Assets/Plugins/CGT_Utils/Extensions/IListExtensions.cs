using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}