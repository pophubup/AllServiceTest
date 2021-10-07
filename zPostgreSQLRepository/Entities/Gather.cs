using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zPostgreSQLRepository.Entities
{
    public class Gather
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int rowid { get; set; }
        public Group Group { get; set; }
        public Label Label { get; set; }
        public ImageFile ImageFile { get; set; }
      
    }

}
