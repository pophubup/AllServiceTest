using System.Collections.Generic;
using System.Threading.Tasks;
using zModelLayer.ViewModels;

namespace zWebCrawlingRepository
{
    public interface IDATOWebCrawling
    {
        public List<EveryPage> GetDataFromWebElement(string value ,int endpage);
    }
}
