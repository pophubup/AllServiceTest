using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;
using Neo4jClient;
using System;
using zModelLayer;

namespace Neo4jClientRepository
{
    public static class Neo4jClient
    {
        public static IServiceCollection AddNeo4jCleint(this IServiceCollection service, Neo4jAuth neo4JAuth)
        {
            service.AddSingleton<IBoltGraphClient>(new BoltGraphClient(
            GraphDatabase.Driver(neo4JAuth.host,
                                 AuthTokens.Basic(neo4JAuth.user,
                                 neo4JAuth.pwd))
            ));
            return service;
        }
    }
}
