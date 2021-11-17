using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using zGrapQLRepository.GraphQL;

namespace zGrapQLRepository
{
    public static class GraphQLClient
    {
        public static IServiceCollection AddGrapQLClient(this IServiceCollection service, string conn)
        {
            service.AddDbContext<MYGraphQLContext>(opt => opt.UseNpgsql(conn));
            service.AddGraphQLServer().AddQueryType<Query>();
            var builder = service.BuildServiceProvider();
            var dbbase = builder.GetService<MYGraphQLContext>();
            var groups = dbbase.Groups.ToListAsync().GetAwaiter().GetResult();
          
            return service;
        }
    }
}
