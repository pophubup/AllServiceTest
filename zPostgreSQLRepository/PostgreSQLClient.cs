using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using zPostgreSQLRepository.Entities;
using zPostgreSQLRepository.Entities_jsonb;

namespace zPostgreSQLRepository
{
    public static class PostgreSQLClient
    {
        public static IServiceCollection AddPostgreSQLClient(this IServiceCollection service, IConfiguration configuration)
        {
    //        service.AddDbContext<TestContext>(options => options
    //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
    //.UseNpgsql(configuration["SQL:NpqSQLConn"]));
            service.AddDbContext<Test2Context>(options => options
   .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
   .UseNpgsql(configuration["SQL:NpqSQLConn2"]));
            var buidler = service.BuildServiceProvider();
            //buidler.GetService<TestContext>().Database.EnsureDeleted();
           // buidler.GetService<Test2Context>().Database.EnsureCreated();
            //buidler.GetService<Test2Context>().Database.EnsureDeleted();
            buidler.GetService<Test2Context>().Database.EnsureCreated();
            //var data = buidler.GetService<Test2Context>().AssignGroups.ToList();
            //var dd = new AssignCategory()
            //{

            //    BucketId = "cccccccccccccc",
            //    CreateDate = DateTime.Now,
            //    LabelName = "ccccccccccc",
            //    assignGroups = data

            //};


            //dd.assignGroups = dddd;
            //var file = new Image()
            //{
            //    FileName = "123",
            //    CreateDate = DateTime.Now,
            //    Description = "fffff",
            //    AssignCategory = dd
            //};
            //buidler.GetService<Test2Context>().AssignCategory.AddRange(dd);
            //buidler.GetService<Test2Context>().Images.Add(file);
            //buidler.GetService<Test2Context>().SaveChanges();
            //var vvv = buidler.GetService<Test2Context>().Images.Include(g => g.AssignCategory).ToList();
            return service;
        }
    }
}
