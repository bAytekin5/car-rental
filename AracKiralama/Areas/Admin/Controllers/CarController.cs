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
	public class CarController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _he;
		public CarController(
			ApplicationDbContext context,
			IWebHostEnvironment he)
		{
			_context = context;
			_he = he;
		}
		public async Task<IActionResult> Index()
		{
			var data = await _context.Car
				.AsNoTracking()
				.ToListAsync();
			return View(data);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(Car p)
		{
			p.UpdatedTime = DateTime.Now;
			var files = HttpContext.Request.Form.Files;
			if (files.Count == 1)
			{
				var ext = Path.GetExtension(files[0].FileName).ToLower();
				string fileName = Guid.NewGuid().ToString();
				var upload = Path.Combine(_he.WebRootPath, @"images");
				using (var filesStreams = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
				{
					files[0].CopyTo(filesStreams);
					p.Image = @"/images/" + fileName + ext;
				}
			}
			await _context.AddAsync(p);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id)
		{
			var data = await _context.Car
				.Where(x => x.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();
			return View(data);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(Car p)
		{
			p.UpdatedTime = DateTime.Now;
			var files = HttpContext.Request.Form.Files;
			if (files.Count == 1)
			{
				var ext = Path.GetExtension(files[0].FileName).ToLower();
				string fileName = Guid.NewGuid().ToString();
				var upload = Path.Combine(_he.WebRootPath, @"images");
				using (var filesStreams = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
				{
					files[0].CopyTo(filesStreams);
					p.Image = @"/images/" + fileName + ext;
				}
			}
			_context.Update(p);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			var data = await _context.Car
				.Where(x => x.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();
			_context.Remove(data);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
