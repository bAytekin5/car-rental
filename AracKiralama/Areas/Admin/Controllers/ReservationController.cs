using AracKiralama.Models;
using AracKiralama.Models.Data;
using AracKiralama.Models.StaticClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = Role.Role_Admin)]
	public class ReservationController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _he;
		public ReservationController(
			ApplicationDbContext context,
			IWebHostEnvironment he)
		{
			_context = context;
			_he = he;
		}
		public async Task<IActionResult> Index()
		{
			var data = await _context.Reservation
				.Include(x=>x.Car)
				.AsNoTracking()
				.ToListAsync();
			return View(data);
		}
		public async Task<IActionResult> Delete(int id)
		{
			var data = await _context.Reservation
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();
			_context.Remove(data);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
