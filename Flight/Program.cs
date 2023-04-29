using Flight.Data;
using FlightAgency.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var identitycs = builder.Configuration.GetConnectionString("Identity");
builder.Services.AddDbContext<IdentityContext>(opts => opts.UseSqlServer(identitycs));
builder.Services.AddIdentity<IdentityUser,IdentityRole>(opts => 
{ 
opts.Password.RequireDigit = false;
    opts.Password.RequiredLength = 5;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<IdentityContext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(new FlightDB(builder.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/Error");

app.Run();
