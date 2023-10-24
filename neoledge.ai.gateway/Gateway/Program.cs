using Gateway.Data;
using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<TextDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 5;

}).AddEntityFrameworkStores<TextDbContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))

    };
}
    );

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add the CORS policy here
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "SimpleUser" };
    foreach (var role in roles)
    {
        if (!await roleManger.RoleExistsAsync(role))
            await roleManger.CreateAsync(new IdentityRole(role));

    }
}
app.Run();
