using Microsoft.AspNetCore.Identity;

namespace AracKiralama.Models
{
	public class AppUser : IdentityUser
	{
		public string NameSurname { get; set; }
	}
}
