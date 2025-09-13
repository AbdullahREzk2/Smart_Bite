using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.OthersServiceManager
{
    public interface IOthersServiceManeger
    {
        public bool AddRate(int UserID, int Rate);
    }
}
