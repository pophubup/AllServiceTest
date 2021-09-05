using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLClientRepository.Entities;
using Microsoft.EntityFrameworkCore;
using SQLClientRepository.IServices;
using SQLClientRepository.Services;

namespace SQLClientRepository
{
    public static class MSSQLCleint
    {
        public static IServiceCollection AddMSQLjCleint(this IServiceCollection service, string connectionstring)
        {
            service.AddDbContext<MYDBContext>(options => options
    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
    .UseSqlServer(connectionstring));
            service.AddScoped<ILabel, LabelService>();
            return service;
        }
    }
}
