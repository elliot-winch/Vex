public static class MathsUtility
{
    public static double forgiveness = 0.00001;

    public static bool RoughlyEquals(this double x, double y)
    {
        double a = x - y;
        return Between(a, - forgiveness, forgiveness, false);
    }

    public static bool Between(double x, double min, double max, bool strictly = false)
    {
        return strictly ? (x - min > 0 && x - max < 0) : (x - min >= 0 && x - max <= 0);
    }
}
