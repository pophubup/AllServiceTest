using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using zGrapQLRepository.GraphQL;
using zPostgreSQLRepository.Entities_jsonb;

namespace zGrapQLRepository
{
    public static class GraphQLClient
    {
        public static IServiceCollection AddGrapQLClient(this IServiceCollection service, string conn)
        {
        
            service.AddGraphQLServer().AddQueryType<Query>();
            var builder = service.BuildServiceProvider();

            return service;
        }
    }
}
