using AracKiralama.Models;
using AracKiralama.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Net;
using System.Net.Mail;

namespace AracKiralama.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toast;
        public HomeController(ApplicationDbContext context, IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public async Task<IActionResult> Cars()
        {
            var data = await _context.Car
                .AsNoTracking()
                .ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> CarDetail(int id)
        {
            var data = await _context.Car
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return View(data);
        }
        public async Task<IActionResult> Pricing()
        {
            var data = await _context.Car
                .AsNoTracking()
                .ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Booking(int id)
        {
            ViewBag.Data = await _context.Car
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Booking(Reservation p)
        {
            var existingReservations = await _context.Reservation
                .Where(r => r.CarId == p.CarId &&
                            r.EndDate >= p.StartDate &&
                            r.StartDate <= p.EndDate)
                .ToListAsync();

            // Çakışan bir rezervasyon var
            if (existingReservations.Any())
            {
                _toast.AddErrorToastMessage("Seçtiğiniz tarihlerde araç için rezervasyon yapılmış");
                return RedirectToAction("Booking", new { id = p.CarId });
            }
            p.CreatedTime = DateTime.Now;
            if (p.StartDate < DateTime.Now || p.EndDate < DateTime.Now)
            {
                _toast.AddErrorToastMessage("Geçerli bir tarih giriniz");
                return RedirectToAction("Booking", new { id = p.CarId });

            }
            if (p.StartDate > p.EndDate)
            {
                _toast.AddErrorToastMessage("Başlangıç tarihi, bitiş tarihinden büyük olamaz");
                return RedirectToAction("Booking", new { id = p.CarId });

            }
            // Çakışan rezervasyon yok
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Rezervasyon yapıldı");
            return RedirectToAction("Booking");
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string NameSurname, string Email, string Konu, string Mesaj)
        {
            MailMessage msg = new MailMessage();
            msg.Subject = "Yeni bir iletişim mesajı";
            msg.From = new MailAddress("testt@berkayaytkn.com", "Araç Kirala");
            msg.To.Add(new MailAddress("berkayaytkn00@gmail.com", "Admin"));
            msg.IsBodyHtml = true;
            msg.Body =
                "Ad Soyad: " + NameSurname + "<br>"
                + "Email: " + Email + "<br>"
                + "Konu: " + Konu + "<br><br>"
                + "Mesaj: " + Mesaj;
            msg.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient("berkayaytkn.com", 587);
            NetworkCredential AccountInfo = new NetworkCredential("testt@berkayaytkn.com", "Admin+123123");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = AccountInfo;
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(msg);
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Contact");
        }
        public IActionResult Error1(int code)
        {
            return View();
        }
    }
}
