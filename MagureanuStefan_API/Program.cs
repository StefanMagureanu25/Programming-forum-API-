using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Repositories;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace MagureanuStefan_API
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
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
    " Enter 'Bearer' [space] and then your token in the text input below." +
    "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            var clubLibraConnectionString = builder.Configuration.GetConnectionString("ClubLibraConnection") ?? throw new InvalidOperationException("Connection string 'ClubLibraConnection' not found.");
            builder.Services.AddDbContext<ClubLibraDataContext>(options =>
                options.UseSqlServer(clubLibraConnectionString));

            //register repositories
            builder.Services.AddTransient<IAnnouncementsRepository, AnnouncementsRepository>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ICodeSnippetsRepository, CodeSnippetsRepository>();
            builder.Services.AddTransient<IMembersRepository, MembersRepository>();
            builder.Services.AddTransient<IMembershipTypesRepository, MembershipTypesRepository>();

            builder.Logging.AddLog4Net("log4net.config");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Domain"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Secret"])
                    )
                    };
                });


            builder.Services.AddAuthorization();
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


            app.MapControllers();

            app.Run();
        }
    }
}