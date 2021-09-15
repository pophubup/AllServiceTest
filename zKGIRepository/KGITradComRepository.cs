using Smart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zKGIRepository.Models;

namespace zKGIRepository
{
    public class KGITradComRepository : IKGITradeCom
    {
        public List<string> GetProcuctBaseList()
        {
            throw new NotImplementedException();
        }

        public List<resP001802> GetProcuctDetailList(string comidname)
        {
            throw new NotImplementedException();
        }

        public ResP001801 GetProductBase(string Symbol)
        {
            throw new NotImplementedException();
        }

        public TaiFexCom InitalClient(string Port, string Id, string Pwd, string Center = "")
        {
            TaiFexCom tfcom = new TaiFexCom("10.4.99.71", 8000, "API");
            throw new NotImplementedException();
        }
    }
}
