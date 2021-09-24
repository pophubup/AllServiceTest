using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zWebCrawlingRepository
{
    #region 鉅亨網股票現值
    public class AnuaCurrent
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public Quote quote { get; set; }
    }
    public class Quote
    {
        /// <summary>
        /// Stock Name 總名
        /// </summary>
        public string _0 { get; set; }
        /// <summary>
        /// 現在價格
        /// </summary>
        public double _6 { get; set; }
        public double _11 { get; set; }
        /// <summary>
        /// 1日最高價格
        /// </summary>
        public double _12 { get; set; }

        /// <summary>
        /// 1日最低價格
        /// </summary>
        public double _13 { get; set; }
        public double _21 { get; set; }
        public double _56 { get; set; }
        public double _75 { get; set; }
        public double _76 { get; set; }
        public double _3404 { get; set; }
        /// <summary>
        /// Stock 中文名
        /// </summary>
        public string _200009 { get; set; }
        public double _800001 { get; set; }
        public string _800013 { get; set; }
        public int _800041 { get; set; }
        public int isTrading { get; set; }
    }
    #endregion
    #region 三大法人購買紀錄
    public class StockPastRecordBasedOnBuyers
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
    }
    public class Datum
    {
        public string code { get; set; }
        public string date { get; set; }
        public int dealerBuyVolume { get; set; }
        public int dealerSellVolume { get; set; }
        public int dealerNetBuySellVolume { get; set; }
        public int foreignBuyVolume { get; set; }
        public int foreignSellVolume { get; set; }
        public int foreignNetBuySellVolume { get; set; }
        public int domesticBuyVolume { get; set; }
        public int domesticSellVolume { get; set; }
        public int domesticNetBuySellVolume { get; set; }
        public int totalNetBuySellVolume { get; set; }
    }
    #endregion



    public class AnuaWebCrawlingRepository : IAnua
    {
        
        public StockPastRecordBasedOnBuyers GetPastRecordBasedOnBuyersByTimeSpan(string idorName, DateTime start, DateTime end)
        {
            long spanStart = ((DateTimeOffset)start).ToUnixTimeSeconds();
            long spanEnd = ((DateTimeOffset)end).ToUnixTimeSeconds();
            //法人區間資料
            //https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A1303%3ASTOCK?from=1632528000&to=1609459200
            throw new NotImplementedException();
        }

        public AnuaCurrent GetCurrentValueFromSpectificStock(string idorName)
        {
            //股票現行資料
            //https://ws.api.cnyes.com/ws/api/v1/charting/history?resolution=1&symbol=TWS:1303:STOCK&quote=1
          
            throw new NotImplementedException();
        }

        public StockPastRecordBasedOnBuyers GetPastRecordWitinWeek(string idorName)
        {
            //法人近一周的資料
            //https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A1303%3ASTOCK/6
            throw new NotImplementedException();
        }
      
     
     
    }
}
