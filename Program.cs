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

builder.Services.AddScoped<IRecoveryPasswordService, RecoveryPasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SchedulingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAuthenticationUserService, AuthenticationUserService>();
builder.Services.AddScoped<ServicesProvidedService>();

// Autenticação por Cookie (e Google, se usar)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(o =>
{
    o.LoginPath = "/Login/Login";
    o.AccessDeniedPath = "/Login/Login";
    o.ExpireTimeSpan = TimeSpan.FromDays(30);
    o.SlidingExpiration = true;
    o.Cookie.HttpOnly = true;
    o.Cookie.Name = ".BarberShop.Login";
    o.Cookie.SameSite = SameSiteMode.Lax;
#if !DEBUG
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS em produção
#else
    o.Cookie.SecurePolicy = CookieSecurePolicy.None;   // permite HTTP em DEV
#endif
})
.AddGoogle(googleOptions =>
{

});

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

// ORDEM CORRETA:
app.UseSession();          // 1) habilita Session
app.UseAuthentication();   // 2) popula HttpContext.User com o cookie, se existir

// REIDRATAR A SESSION A PARTIR DO COOKIE usando as MESMAS chaves do SessionHelper
app.Use(async (ctx, next) =>
{
    if (ctx.User?.Identity?.IsAuthenticated == true)
    {
        // Se a sessão ainda não tem "Id" (chave usada pelo SessionHelper), reidrate
        var currentId = ctx.Session.GetInt32("Id");
        if (currentId is null or 0)
        {
            var userIdStr = ctx.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var name = ctx.User.Identity?.Name ?? "";
            var role = ctx.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "";

            if (int.TryParse(userIdStr, out var userId) && userId > 0)
            {
                ctx.Session.SetInt32("Id", userId);           // <— mesma chave do SessionHelper
                ctx.Session.SetString("Name", name);          // <— mesma chave do SessionHelper
                ctx.Session.SetString("TipoUsuario", role);   // <— mesma chave do SessionHelper
            }
        }
    }
    await next();
});

app.UseAuthorization();    // 3)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
