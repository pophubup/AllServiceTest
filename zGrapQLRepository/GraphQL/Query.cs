using HotChocolate;
using System.Linq;
using zPostgreSQLRepository.Entities_jsonb;

namespace zGrapQLRepository.GraphQL
{
    public class Query
    {
        public IQueryable<AssignGroup> GetAssignGroups([Service] Test2Context context)
        {
            return context.AssignGroups;
        }
        public IQueryable<Image> GetImage([Service] Test2Context context)
        {
            return context.Images;
        }
        public IQueryable<AssignCategory> GetAssignCategory([Service] Test2Context context)
        {
            return context.AssignCategory;
        }
    }
}
