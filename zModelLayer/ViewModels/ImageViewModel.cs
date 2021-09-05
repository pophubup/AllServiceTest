using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer.ViewModels
{
    public class ImageViewModel
    {

        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public int LabelId { get; set; }
        public int GroupId { get; set; }

    }
}
