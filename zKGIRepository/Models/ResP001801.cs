using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zKGIRepository.Models
{
    public class ResP001801
    {
        /// <summary>
        /// 商品代碼(Ex:TXO)
        /// </summary>
        public string ComId { get; set; }
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
        /// 細項類別 1.指數 2.個股 3.利率 4.債券 5.國外 6.美元計價 7.人民幣計價商品 8. 日幣計價商品
        /// </summary>
        public char ContractType { get; set; }
        /// <summary>
        /// 交易基數
        /// </summary>
        public decimal ContractValue { get; set; }
        /// <summary>
        ///  交易稅
        /// </summary>
        public decimal TaxRate { get; set; }
        /// <summary>
        ///  最小跳動點
        /// </summary>
        public decimal Tick { get; set; }
        /// <summary>
        /// 商品中文名稱
        /// </summary>
        public string ComCName { get; set; }

     
    }
}
