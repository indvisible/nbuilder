using System.Collections.Generic;
using System.Linq;

namespace Indvisible.RealisticData.Extensions
{
    public static class EnumerableExtensions
    {
        public static string Join<T>(this IEnumerable<T> items, string separator)
        {
            return string.Join(separator, items);
        }

        public static T Rand<T>(this IEnumerable<T> items)
        {
            IList<T> list;
            if (items is IList<T>)
            {
                list = (IList<T>) items;
            }
            else
            {
                list = items.ToList();
            }

            return list[DataRandom.Rand.Next(list.Count)];
        }

        public static IEnumerable<T> RandPick<T>(this IEnumerable<T> items, int itemsToTake)
        {
            IList<T> list;
            if (items is IList<T>)
            {
                list = (IList<T>) items;
            }
            else
            {
                list = items.ToList();
            }

            var rand = DataRandom.Rand;

            for (var i = 0; i < itemsToTake; i++)
            {
                yield return list[rand.Next(list.Count)];                
            }
        }

        /// <summary>
        /// From here:
        /// http://stackoverflow.com/questions/375351/most-efficient-way-to-randomly-sort-shuffle-a-list-of-integers-in-c
        /// </summary>
        public static IList<T> Shuffle<T>(this IList<T> array)
        {
            var retArray = new T[array.Count];
            array.CopyTo(retArray, 0);

            var random = DataRandom.Rand;
            for (var i = 0; i < array.Count; i += 1)
            {
                var swapIndex = random.Next(i, array.Count);
                if (swapIndex == i)
                {
                    continue;
                }

                var temp = retArray[i];
                retArray[i] = retArray[swapIndex];
                retArray[swapIndex] = temp;
            }

            return retArray;
        }
    }
}