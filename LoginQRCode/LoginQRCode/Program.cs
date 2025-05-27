using LoginQRCode;
using LoginQRCode.Data;
using LoginQRCode.Entities;
using LoginQRCode.Mapper;
using LoginQRCode.Services;
using LoginQRCode.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                      });
});

builder.Services.AddSingleton<PresenceTracker>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<ITokenService, TokenService>();

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
})
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoles<IdentityRole>()//cai nay truoc AddEntityFrameworkStores
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"].ToString())),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

    op.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
//-----------------
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        await context.Database.MigrateAsync();
        await Seed.SeedUsers(userManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred during migration");
    }
}

app.MapHub<PresenceHub>("hubs/presence");
//app.MapFallbackToController("Index", "Fallback");

app.Run();
