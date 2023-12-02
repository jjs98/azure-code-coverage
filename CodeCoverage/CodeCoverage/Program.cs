using CodeCoverage.Handler;
using CodeCoverage.Interfaces.Handler;
using CodeCoverage.Interfaces.Services;
using CodeCoverage.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeCoverage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var accessToken = builder.Configuration["Settings:AzureTokenAll"];
            builder.Services.AddSingleton<IHttpService>(new HttpService(accessToken));
            builder.Services.AddScoped<IAzureService, AzureService>();
            builder.Services.AddScoped<ICodeCoverageHandler, CodeCoverageHandler>();
            builder.Services.AddScoped<IPullRequestsHandler, PullRequestsHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
