using Microsoft.EntityFrameworkCore;

namespace zPostgreSQLRepository.Entities_jsonb
{
    public class Test2Context : DbContext
    {
        public virtual DbSet<AssignGroup> AssignGroups { get; set; }
        public virtual DbSet<AssignCategory> AssignCategory { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public Test2Context(DbContextOptions<Test2Context> options)
         : base(options)
        {
        }
    }
}
