using System.ComponentModel.DataAnnotations;

namespace  StockPerformance_CleanArchitecture.Models.EmailDetails
{
    public class  EmailContact
    {
        [EmailAddress]
		public string EmailAddress { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }    
        
    }
}