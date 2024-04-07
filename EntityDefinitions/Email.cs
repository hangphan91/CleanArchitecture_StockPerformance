using System;
using System.ComponentModel.DataAnnotations;

namespace EntityDefinitions
{
	public class Email : IdBase
    {
		[EmailAddress]
		public string EmailAddress { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}

