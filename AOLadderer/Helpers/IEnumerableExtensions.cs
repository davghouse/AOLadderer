using System.Collections.Generic;

namespace AOLadderer.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> values, T value)
        {
            yield return value;

            foreach (T item in values)
                yield return item;
        }
    }
}
