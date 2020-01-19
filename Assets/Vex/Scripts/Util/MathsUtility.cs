using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class MathsUtility
{
    public static bool RoughlyEquals(this double x, double y, double forgiveness = 0.00001)
    {
        return Between(x - y, -forgiveness, forgiveness, false);
    }

    public static bool Between(double x, double min, double max, bool strictly = false)
    {
        return strictly ? (x > min && x < max) : (x >= min && x <= max);
    }

    public static Vector3 Midpoint(params Vector3[] ps)
    {
        return Midpoint(ps.ToList());
    }

    public static Vector3 Midpoint(List<Vector3> p)
    {
        return p.Aggregate((x, y) => x + y) / p.Count;
    }
}
