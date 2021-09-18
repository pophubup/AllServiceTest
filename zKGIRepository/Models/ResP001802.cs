using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zKGIRepository.Models
{
    public class resP001802
    {
        /// <summary>
        /// 商品類別F:期貨 O: 選擇權
        /// </summary>
        public char ComType { get; set; }
        /// <summary>
        /// 價格小數位
        /// </summary>
        public int PriceDecimal { get; set; }
        /// <summary>
        /// 履約價格小數位
        /// </summary>
        public int StkPriceDecimal { get; set; }
        /// <summary>
        /// 熱門(近一月)
        /// </summary>
        public bool Hot { get; set; }
        /// <summary>
        ///漲停價
        /// </summary>
        public decimal RisePrice { get; set; }
        /// <summary>
        ///跌停價
        /// </summary>
        public decimal FallPrice { get; set; }
        /// <summary>
        ///商品代碼     交易所完整20碼  例：TXO08900C1 
        /// </summary>
        public string  ComId { get; set; }

    }
}
