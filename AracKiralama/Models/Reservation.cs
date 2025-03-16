using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AracKiralama.Models
{
	public class Reservation
	{
		[Key]
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CreatedTime { get; set; }
		public double? TotalPrice { get; set; }
		public string NameSurname { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Description { get; set; }
		[ForeignKey("Car")]
		public int? CarId { get; set; }
		public Car? Car { get; set; }

	}
}
