using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectAlfa.ApplicationDbContext;
using ProjectAlfa.Entities;
using ProjectAlfa.SendEmail;

namespace ProjectAlfa;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);


		// Add services to the container.
		builder.Services.AddControllersWithViews();

		builder.Services.AddRazorPages();
		
		builder.Services.AddSingleton<IEmailSender,EmailSender>();

		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddIdentity<User, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
			})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

		builder.Services.ConfigureApplicationCookie(options =>
		{
			options.LoginPath = "/Identity/Account/Login"; 
		});
		

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		app.UseHttpsRedirection();
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}


		app.UseStaticFiles();
		

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();


		app.MapControllers();
		app.MapRazorPages();


		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}