using AracKiralama.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.ViewComponents
{
	public class RandomCarsForIndex : ViewComponent
	{
		private readonly ApplicationDbContext _db;
		public RandomCarsForIndex(ApplicationDbContext db)
		{
			_db = db;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var totalCount = await _db.Car.CountAsync();
			var randomIndexes = GetRandomIndexes(Convert.ToInt32(totalCount), 3);
			var randomBoats = await _db.Car
				.Where(b => randomIndexes.Contains(b.Id))
				.ToListAsync();
			return View(randomBoats);
		}
		private List<int> GetRandomIndexes(int totalCount, int count)
		{
			var random = new Random();
			var indexes = new HashSet<int>();

			count = Math.Min(count, totalCount);

			while (indexes.Count < count)
			{
				var index = random.Next(1, totalCount + 1);
				indexes.Add(index);
			}

			return indexes.ToList();
		}
	}
}