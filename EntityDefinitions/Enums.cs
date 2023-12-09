namespace EntityDefinitions
{
    public enum PositionType
    {
        Bought = 0,
        Sold = 1,
        Holding = 2,
        Closed = 3
    }

    public enum SellReasonType
    {
        NotSpecified = 0,
        SellingPercentageRule = 1,
        IsTooVolatile = 2,
        SellingDollarLimitationRule = 3,
    }
}

