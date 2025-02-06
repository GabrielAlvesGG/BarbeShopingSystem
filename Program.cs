using BarberShopSystem.Data;
using BarberShopSystem.ModelsRepository;
using BarberShopSystem.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

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
builder.Services.AddScoped<RecoveryPasswordService>();
builder.Services.AddScoped<EmailService>(); 
builder.Services.AddScoped<RecoveryPasswordService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SchedulingRepository>();
builder.Services.AddScoped<SchedulingService>();
builder.Services.AddScoped<AppointmentsRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<CustomerService>();




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