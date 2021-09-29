using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace zWebCrawlingRepository
{
    #region 鉅亨網股票現值
    public class AnuaCurrent
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string s { get; set; }
        public List<int> t { get; set; }
        public List<double> o { get; set; }
        public List<double> h { get; set; }
        public List<double> l { get; set; }
        public List<double> c { get; set; }
        public List<double> v { get; set; }
        public List<object> vwap { get; set; }
        public Quote quote { get; set; }
        public List<List<int>> session { get; set; }
        public int nextTime { get; set; }
    }
    public class Quote
    {
        [JsonProperty(propertyName: "0")]
        public string _0 { get; set; }
        [JsonProperty(propertyName: "800013")]
        public string _800013 { get; set; }
        [JsonProperty(propertyName: "800041")]
        public int _800041 { get; set; }
        [JsonProperty(propertyName: "isTrading")]
        public int isTrading { get; set; }
        [JsonProperty(propertyName: "6")]
        public double _6 { get; set; }
        [JsonProperty(propertyName: "200009")]
        public string _200009 { get; set; }
        [JsonProperty(propertyName: "75")]
        public double _75 { get; set; }
        [JsonProperty(propertyName: "11")]
        public double _11 { get; set; }
        [JsonProperty(propertyName: "76")]
        public double _76 { get; set; }
        [JsonProperty(propertyName: "12")]
        public double _12 { get; set; }
        [JsonProperty(propertyName: "3404")]
        public double _3404 { get; set; }
        [JsonProperty(propertyName: "13")]
        public double _13 { get; set; }
        [JsonProperty(propertyName: "800001")]
        public double _800001 { get; set; }
        [JsonProperty(propertyName: "21")]
        public double _21 { get; set; }
        [JsonProperty(propertyName: "56")]
        public double _56 { get; set; }
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
            TimeSpan ts = new TimeSpan(8, 0, 0);
            start = Convert.ToDateTime($"{ start.Date.ToString("yyyy/MM/dd")} {ts.Hours}:{ts.Minutes}");
            end = Convert.ToDateTime($"{ end.Date.ToString("yyyy/MM/dd")} {ts.Hours}:{ts.Minutes}");
            long spanStart = ((DateTimeOffset)start).ToUnixTimeSeconds();
            long spanEnd = ((DateTimeOffset)end).ToUnixTimeSeconds();

            //法人區間資料
            //https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A1303%3ASTOCK?from=1632528000&to=1609459200
            var result = new HttpClient().GetAsync($"https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A{idorName}%3ASTOCK?from={spanEnd}&to={spanStart}").GetAwaiter().GetResult();
            StockPastRecordBasedOnBuyers target = new StockPastRecordBasedOnBuyers();
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var cotent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                target = JsonConvert.DeserializeObject<StockPastRecordBasedOnBuyers>(cotent);
            }
            return target;
        }

        public AnuaCurrent GetCurrentValueFromSpectificStock(string idorName)
        {
            //股票現行資料
            //https://ws.api.cnyes.com/ws/api/v1/charting/history?resolution=1&symbol=TWS:1303:STOCK&quote=1
            var result = new HttpClient().GetAsync($"https://ws.api.cnyes.com/ws/api/v1/charting/history?resolution=1&symbol=TWS:{idorName}:STOCK&quote=1").GetAwaiter().GetResult();
            AnuaCurrent target = new AnuaCurrent();
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var cotent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
              
                 target = JsonConvert.DeserializeObject<AnuaCurrent>(cotent);
            }
            return target;
        }

        public StockPastRecordBasedOnBuyers GetPastRecordWitinWeek(string idorName)
        {
            //法人近一周的資料
            //https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A1303%3ASTOCK/6
            var result = new HttpClient().GetAsync($"https://marketinfo.api.cnyes.com/mi/api/v1/investors/buysell/TWS%3A{idorName}%3ASTOCK/6").GetAwaiter().GetResult();
            StockPastRecordBasedOnBuyers target = new StockPastRecordBasedOnBuyers();
            if (result.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                var cotent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                target = JsonConvert.DeserializeObject<StockPastRecordBasedOnBuyers>(cotent);
            }
            return target;
        }
      
     
     
    }
}
