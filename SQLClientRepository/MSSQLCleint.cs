using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SQLClientRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace SQLClientRepository
{
    public static class MSSQLCleint
    {
        public static IServiceCollection AddMSQLjCleint(this IServiceCollection service)
        {
            service.AddDbContext<MYDBContext>(options => options
    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
    .UseSqlServer("Server=35.239.20.109;Database=MYDB;User Id=home7996;Password=Home2426;"));
            return service;
        }
    }
}
