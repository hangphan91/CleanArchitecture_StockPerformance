using System;
using System.ComponentModel.DataAnnotations;

namespace EntityDefinitions
{
	public class Email : IdBase
    {
		[EmailAddress]
		public string EmailAddress { get; set; }

		public string FistName { get; set; }

		public string LastName { get; set; }
	}
}

