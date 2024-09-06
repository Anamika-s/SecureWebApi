
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureWebApi.Context;
using SecureWebApi.IRepository;
using System.Text;
using WebApiUsingRepoPattern.Repository;

namespace SecureWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            // AddScoped  > there will be one instance for request by one user
            // AddSingleton > there will be one instance for all requests
            // AddTransient > there will be one instance for every request

            builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("TaskConnection")));

            // telling what authentication scheme we are using
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
  ValidateIssuer = true,
  ValidateAudience = true,
  ValidateLifetime = true,
  ValidateIssuerSigningKey = true,
  ValidIssuer = builder.Configuration["Jwt:Issuer"],
  ValidAudience = builder.Configuration["Jwt:Audience"],
  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
};
});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
