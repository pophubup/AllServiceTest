using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zPostgreSQLRepository.Entities
{
    public class TestContext : DbContext
    {
        public virtual DbSet<Gather> Gathers { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<ImageFile> ImageFiles { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        {
        }
      
    }
}
