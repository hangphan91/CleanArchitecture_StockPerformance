namespace OptionPerformance.Models.Interfaces.Logics
{
    public interface ICalculatingOptionsReturnBase
    {
        decimal CalculateMaxRisk();
        decimal CalculateMaxReward();
        decimal? ActualReturn(decimal stockPrice);
        decimal? CurrentUnrealizedGain(decimal stockPrice);
        decimal MarginCollateral();
    }
}