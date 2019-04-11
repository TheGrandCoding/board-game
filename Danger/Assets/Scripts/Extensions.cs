using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Extensions
{
    public static class Extensions {
        /// <summary>
        /// Only adds the item to list if it is not already in it
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="list">List to add, or not add, to</param>
        /// <param name="item">Item to add, or not add, to</param>
        public static void UniqueAdd<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }
    }
}
