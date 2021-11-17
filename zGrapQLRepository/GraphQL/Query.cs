using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zGrapQLRepository.GraphQL
{
    public class Query
    {
        public IQueryable<Group> GetGroups([Service] MYGraphQLContext context)
        {
            return context.Groups;
        }
    }
}
