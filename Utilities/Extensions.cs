namespace Utilities;

public static class Extensions
{
    public static decimal RoundNumber(this decimal? number)
    {
        if (number == null)
            return 0;

        return Math.Round(number.Value, 2);
    }


    public static decimal RoundNumber(this decimal number)
    {
        return Math.Round(number, 2);
    }
}

