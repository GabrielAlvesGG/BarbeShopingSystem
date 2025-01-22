var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona suporte ao cache e sess�es
builder.Services.AddDistributedMemoryCache(); // Necess�rio para armazenar dados da sess�o
builder.Services.AddSession(); // Habilita sess�es no projeto

// Habilita o HttpContextAccessor para facilitar o uso em outras classes
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Habilita acesso a arquivos est�ticos (CSS, JS, imagens, etc.)

app.UseRouting();

app.UseSession(); // Middleware de sess�o deve ser adicionado aqui
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
