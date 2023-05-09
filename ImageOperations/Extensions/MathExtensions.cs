namespace ImageOperations.Extensions
{
    public static class MathExtensions
    {
        public static double Clamp(double min, double max, double value)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
        public static int Clamp(int min, int max, int value)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}