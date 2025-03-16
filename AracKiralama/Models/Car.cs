using System.ComponentModel.DataAnnotations;

namespace AracKiralama.Models
{
	public class Car
	{

		[Key]
		public int Id { get; set; }
		public DateTime UpdatedTime { get; set; }
		public string? Image { get; set; }
		public string? Brand { get; set; }
		public string? Model { get; set; }
		public double? DailyPrice { get; set; }
		public string? Km { get; set; }
		public string? Transmission { get; set; }
		public string? Seats { get; set; }
		public string? Luggage { get; set; }
		public string? Fuel { get; set; }
		public string? GoogleMapIFrame { get; set; }
		public string? Description { get; set; }
		public List<Reservation>? Reservation { get; set; }


	}
}
