namespace OptionPerformance.Models.Interfaces.OptionsTypes
{
    public interface IWithSellingStock
    {
        int NumberOfSellingStock { get; set; }
        decimal SellingPrice { get; set; }
    }
}