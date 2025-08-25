using System.Collections.Generic;
using UnityEngine;

namespace Internal.Core.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list.Count == 0) return default;

            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static bool HasElements<T>(this List<T> list)
        {
            return list != null && list.Count > 0;
        }
    }
}