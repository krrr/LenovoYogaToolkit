using System;

namespace LenovoYogaToolkit.Lib.Extensions;

public class MathExtensions
{
    public static int RoundNearest(int value, int factor)
    {
        return (int)Math.Round(value / (double)factor, MidpointRounding.AwayFromZero) * factor;
    }
}