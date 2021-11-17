using System;
using System.Collections.Generic;

#nullable disable

namespace zGrapQLRepository
{
    public partial class ImageFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int LabelId { get; set; }

        public virtual Label Label { get; set; }
    }
}
