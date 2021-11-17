using System;
using System.Collections.Generic;

#nullable disable

namespace zGrapQLRepository
{
    public partial class Group
    {
        public Group()
        {
            Labels = new HashSet<Label>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }
}
