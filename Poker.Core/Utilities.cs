using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core.Interfaces;

namespace Poker.Core
{
    static class Utilities
    {
        /// <summary>
        /// Gets the next item in the list, or the first if the current item was the last (or null)
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

        /// <summary>
        /// Gets the next active player
        /// </summary>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        internal static IPlayer NextActiveOrFirst(this IList<IPlayer> list, IPlayer current)
        {
            do
            {
                current = list.NextAfterOrFirst(current);
            } while (current.Participation != PlayerParticipation.Active);

            return current;
        }
    }
}
