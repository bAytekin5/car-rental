using AracKiralama.Models;
using AracKiralama.Models.Data;
using AracKiralama.Models.StaticClasses;
using AracKiralama.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = Role.Role_Admin)]
	public class UserController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		public UserController(
			UserManager<IdentityUser> userManager,
			ApplicationDbContext context,
			IMapper mapper)
		{
			_userManager = userManager;
			_context = context;
			_mapper = mapper;
		}
		[HttpGet]
		[Route("/admin")]
		public async Task<IActionResult> UserList()
		{
			var users = await _context.AppUsers.AsNoTracking().ToListAsync();
			var userListVM = new List<UserListVM>();
			foreach (var user in users)
			{
				var userDto = _mapper.Map<UserListVM>(user);
				userDto.Roles = await _userManager.GetRolesAsync(user);
				userListVM.Add(userDto);
			}
			return View(userListVM);
		}
		[HttpGet]
		public IActionResult AddUser()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddUser(RegisterVM registerVM)
		{
			if (ModelState.IsValid)
			{
				var user = new AppUser
				{
					UserName = registerVM.Email,
					Email = registerVM.Email,
					NameSurname = registerVM.NameSurname,
					PhoneNumber = registerVM.PhoneNumber,
				};
				var result = await _userManager.CreateAsync(user, registerVM.Password);
				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, registerVM.Role);
					return RedirectToAction("UserList");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(registerVM);
		}
		public async Task<IActionResult> LockUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			user.LockoutEnd = DateTime.MaxValue;
			await _userManager.UpdateAsync(user);
			return RedirectToAction("UserList");
		}
		public async Task<IActionResult> UnlockUser(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			user.LockoutEnd = null;
			await _userManager.UpdateAsync(user);
			return RedirectToAction("UserList");
		}
		public async Task<IActionResult> ChangeUserRole(string id, string role)
		{
			var user = await _userManager.FindByIdAsync(id);
			var roles = await _userManager.GetRolesAsync(user);
			await _userManager.RemoveFromRolesAsync(user, roles);
			await _userManager.AddToRoleAsync(user, role);
			return RedirectToAction("UserList");
		}
	}
}
