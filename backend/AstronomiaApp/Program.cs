using AstronomiaApp.Data;
using AstronomiaApp.Integracion;
using AstronomiaApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC + Razor Views
builder.Services.AddControllersWithViews();

// Entity Framework Core con SQL Server
builder.Services.AddDbContext<AstronomiaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AstronomiaDB")));

// HttpClient para API externa
builder.Services.AddHttpClient<SolarSystemApiClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Servicios de dominio
builder.Services.AddScoped<ObjetoService>();
builder.Services.AddScoped<GrafoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Ruta por defecto → ObjetosController.Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Objetos}/{action=Index}/{id?}");

// Habilitar atributos [ApiController]
app.MapControllers();

app.Run();
