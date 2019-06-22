using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static IEnumerable<T> NonNull<T>(this IEnumerable<T> v)
    {
        return v.Where(x => x != null);
    }
}
