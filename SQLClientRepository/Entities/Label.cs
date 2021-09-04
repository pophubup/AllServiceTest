using System;
using System.Collections.Generic;

#nullable disable

namespace SQLClientRepository.Entities
{
    public partial class Label
    {
        public Label()
        {
            ImageFiles = new HashSet<ImageFile>();
        }

        public int Id { get; set; }
        public string LabelName { get; set; }
        public DateTime CreateDate { get; set; }
        public int GroupId { get; set; }
        public string BucketId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ICollection<ImageFile> ImageFiles { get; set; }
    }
}
