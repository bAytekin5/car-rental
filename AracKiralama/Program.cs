using AracKiralama.Mapping;
using AracKiralama.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(_ =>
{
	_.Password.RequiredLength = 5;
	_.Password.RequireNonAlphanumeric = false;
	_.Password.RequireLowercase = false;
	_.Password.RequireUppercase = false;
	_.Password.RequireDigit = false;
})
		.AddEntityFrameworkStores<ApplicationDbContext>()
		.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.LoginPath = "/Identity/Account/Login";
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        CloseButton = true,
        PositionClass = ToastPositions.TopRight,
        PreventDuplicates = true,
        TimeOut = 4000,
    }, new NToastNotifyOption
    {
        DefaultSuccessTitle = "BAÞARILI",
        DefaultErrorTitle = "HATA",
    });
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/User/Home/Error1", "?code={0}");
app.UseStaticFiles(new StaticFileOptions
{
	OnPrepareResponse = ctx =>
	{
		ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800");
	}
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.Run();
