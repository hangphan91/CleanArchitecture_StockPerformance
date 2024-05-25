namespace OptionPerformance.Models.Interfaces.Logics
{
    public interface IOptionsTradingMaster : ICalculatingOptionsReturnBase, IExitOptionsSetup, IEnteringOptionsSetup
    {
        OptionsLegs OptionsLegs { get; }
    }
}