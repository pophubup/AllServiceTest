using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer.ViewModels
{
    public class GroupData
    {
        public int index { get; set; }
        public int groupID { get; set; }
        public string groupName { get; set; }
        public DateTime CreateDate { get; set; }
        public List<LableViewModel> LableViewModels { get; set; }
     
    }

    public class LableViewModel
    {
        public int index { get; set; }
        public int labelId { get; set; }
        public string labelName { get; set; }
        public DateTime createDate { get; set; }
        public List<ImageData> imageDatas {get;set;}
    }
    public class ImageData
    {
        public string description { get; set; }
        public string fileName { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public int labelId { get; set; }
        public DateTime createDate { get; set; }
    }
  

}
