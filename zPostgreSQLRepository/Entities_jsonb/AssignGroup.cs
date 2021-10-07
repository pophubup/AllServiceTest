using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zPostgreSQLRepository.Entities_jsonb
{
    public class AssignGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
