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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts => {
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey"))),
        ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("Admin", policy => { policy.RequireClaim("Role", "Admin"); });
    opts.AddPolicy("Client", policy => { policy.RequireClaim("Role", "Client"); });

});
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
    // The default HSTS value is 30 days.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/Error");

app.Run();
