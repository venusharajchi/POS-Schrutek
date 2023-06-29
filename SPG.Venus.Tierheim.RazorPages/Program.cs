using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SPG.Venus.Tierheim.Infrastructure;
using SPG.Venus.Tierheim.Repository;
using System.Security.Principal;
using SPG.Venus.Tierheim.Application;


var options = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=Tierheim.db")
    .Options;

using (var db = new TierheimContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.SaveChanges();
}


var builder = WebApplication.CreateBuilder(args);

// Register DB Context
builder.Services.AddDbContext<TierheimContext>(opt => opt.UseSqlite("Data Source=Tierheim.db"));

// Register the repositories
builder.Services.AddTransient<KundeRepository>();
builder.Services.AddTransient<TierheimRepository>();

// Register the Services
builder.Services.AddTransient<KundeValidationService>();
builder.Services.AddTransient<TierheimValidationService>();

builder.Services.AddTransient<TierheimService>();
builder.Services.AddTransient<KundenService>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

