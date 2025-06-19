using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();


builder.Services.AddDbContext<OsiguranjeDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("OsiguranjeDbConnectionString")));
builder.Services.AddDbContext<AuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPolisaRepository, PolisaRepository>();
builder.Services.AddScoped<IUslovOsiguranjaRepository, UslovOsiguranjaRepository>();
builder.Services.AddScoped<IKlijentRepository, KlijentRepository>();
builder.Services.AddScoped<IVoziloRepository, VoziloRepository>();
builder.Services.AddScoped<IProdajaRepository, ProdajaRepository>();
builder.Services.AddScoped<IIzvjestajRepository, IzvjestajRepository>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Izvjestaj}/{action=List}/{id?}");

app.Run();
