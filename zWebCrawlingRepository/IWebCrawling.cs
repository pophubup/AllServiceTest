using System.Collections.Generic;
using System.Threading.Tasks;

namespace zWebCrawlingRepository
{
    public interface IWebCrawling<T> where T :class
    {
   
        public List<T> GetDataFromWebElement(string value ,int endpage);
    }
}
