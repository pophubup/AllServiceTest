using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;
using Neo4jClient;
using System;

namespace Neo4jClientRepository
{
    public static class Neo4jClient
    {
        public static IServiceCollection AddNeo4jCleint(this IServiceCollection service)
        {
            service.AddSingleton<IBoltGraphClient>(new BoltGraphClient(
            GraphDatabase.Driver("bolt://localhost:7687",
                                 AuthTokens.Basic("neouser",
                                 "neo123"))
            ));
            return service;
        }
    }
}
