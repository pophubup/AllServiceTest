using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLClientRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace SQLClientRepository
{
    public static class MSSQLCleint
    {
        public static IServiceCollection AddMSQLjCleint(this IServiceCollection service, string connectionstring)
        {
            service.AddDbContext<MYDBContext>(options => options
    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
    .UseSqlServer(connectionstring));
            return service;
        }
    }
}
