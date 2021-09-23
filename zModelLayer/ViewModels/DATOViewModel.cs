using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zModelLayer.ViewModels
{
    public class EveryPage
    {
        public int page { get; set; }
        public List<DATOViewModel> result { get; set; }
    }
    public class DATOViewModel
    {
        public int index { get; set; }
        public string isWinOrFail { get; set; }
        public string type { get; set; }
        public string finishTime { get; set; }
        public string totalPlayHour { get; set; }
                
    }
}

