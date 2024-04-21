namespace OptionPerformance.Models.Interfaces
{
    public interface IOptionsGreek
    {
        OptionsGreek OptionsGreek { get; }
    }

    public class OptionsGreek
    {
        public OptionsGreek(string riskDesc, decimal delta, decimal gamma, decimal theta, decimal vega, decimal rho)
        {
            RiskDescription = riskDesc;
            Delta = delta;
            Gamma = gamma;
            Theta = theta;
            Vega = vega;
            Rho = rho;
        }
        public string? RiskDescription { get; set; }
        public decimal Delta { get; set; }
        public decimal Gamma { get; set; }
        public decimal Theta { get; set; }
        public decimal Vega { get; set; }
        public decimal Rho { get; set; }
    }
}