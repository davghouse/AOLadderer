using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Helpers
{
    public static class IEnumerableExtensions
    {
        public static bool IsSupersetOf<T>(this IReadOnlyCollection<T> first, IReadOnlyCollection<T> second)
            => first.Count > second.Count && second.All(v => first.Contains(v));

        public static void Swap<T>(this IList<T> values, int firstIndex, int secondIndex)
        {
            var firstValue = values[firstIndex];
            values[firstIndex] = values[secondIndex];
            values[secondIndex] = firstValue;
        }
    }
}
