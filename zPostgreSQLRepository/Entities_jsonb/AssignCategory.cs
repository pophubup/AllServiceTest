using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zPostgreSQLRepository.Entities_jsonb
{
    public class AssignCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int LId { get; set; }
        public string LabelName { get; set; }
        public DateTime CreateDate { get; set; }
        public string BucketId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<AssignGroup> assignGroups { get; set; }
    }
}
