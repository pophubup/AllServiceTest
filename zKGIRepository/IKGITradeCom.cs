using Package;
using Smart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zKGIRepository.Models;

namespace zKGIRepository
{
    public interface IKGITradeCom
    {
        #region domestic
        public TaiFexCom InitalClient(string Port, string Id, string Pwd, string Center ="");
        /// <summary>
        /// 國內期權商品查詢 
        /// 3-2 商品名稱查詢
        /// </summary>
        /// <param name="Symbol">商品代碼(Ex:TXO)。</param>
        /// <returns></returns>
        public ResP001801 GetProductBase(string Symbol);
        /// <summary>
        /// 5 取得所有商品列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcuctBaseList();
        /// <summary>
        /// 6 取得某類別/名稱之商品列表
        /// </summary>
        /// <returns></returns>
        public List<resP001802> GetProcuctDetailList(string comidname);
        #endregion
        #region foregin

        #endregion

    }
}
