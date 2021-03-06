using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zWebCrawlingRepository
{
    public interface IAnua
    {
        public AnuaCurrent GetCurrentValueFromSpectificStock(string idorName);
        public StockPastRecordBasedOnBuyers GetPastRecordBasedOnBuyersByTimeSpan(string idorName, DateTime start, DateTime end);
        public StockPastRecordBasedOnBuyers GetPastRecordWitinWeek(string idorName);
    }
}
