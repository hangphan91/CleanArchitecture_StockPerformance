using OptionPerformance.Models.Interfaces.Logics;

namespace OptionPerformance.Models.Interfaces
{
    public interface IOptionsLeg : ISelectingStock, ISelectingOptions, IOptionsGreek
    {
        OptionsLeg OptionsLeg { get; }
    }

    public class OptionsLeg
    {
        public OptionsLeg(string desc, TrendingDirection trendingDirection, string name, string stockSymbol, int numberOfOptions)
        {
            Descriptions = desc;
            TrendingDirection = trendingDirection;
            OptionName = name;
            StockSymbol = stockSymbol;
            NumberOfOptions = numberOfOptions;
        }

        public string OptionName { get; }
        public string StockSymbol { get; }
        public string Descriptions { get; }
        public int NumberOfOptions { get; }
        public TrendingDirection TrendingDirection { get; }
    }

    public class OptionsLegs
    {
        public OptionsLegs()
        {
            OptionsLegList = new List<IOptionsLeg> { };
        }
        public OptionsLegs(List<IOptionsLeg> optionsLegs)
        {
            OptionsLegList = optionsLegs;
        }

        public OptionsLegs(IOptionsLeg optionsLeg)
        {
            OptionsLegList = new List<IOptionsLeg> { optionsLeg };
        }

        public List<IOptionsLeg> OptionsLegList { get; set; }
    }
}