using BarberShopSystem.Data;
using BarberShopSystem.ModelsRepository;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.WebEncoders.Testing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add support for caching and sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Add database repository
builder.Services.AddScoped<DataBaseRepository>();
builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SchedulingRepository>();
builder.Services.AddScoped<AppointmentsRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<RecoveryPasswordRepository>();
builder.Services.AddScoped<HistoricalSchedulesRepository>();
builder.Services.AddScoped<ServicesProvidedRepository>();

builder.Services.AddScoped<RecoveryPasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>(); 
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SchedulingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAuthenticationUserService, AuthenticationUserService>();
builder.Services.AddScoped<ServicesProvidedService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();