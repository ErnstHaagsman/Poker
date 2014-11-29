using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core
{
    static class Utilities
    {
        /// <summary>
        /// Gets the next item in the list, or the first if the current item was the last
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List of items</param>
        /// <param name="current">The current item</param>
        /// <returns></returns>
        internal static T NextAfterOrFirst<T>(this IList<T> list, T current)
        {
            int newIndex = list.IndexOf(current);
            newIndex++;
            if (newIndex == list.Count)
                newIndex = 0;

            return list[newIndex];
        }
    }
}
