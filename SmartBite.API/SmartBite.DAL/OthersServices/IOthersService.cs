using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.DAL.OthersServices
{
    public interface IOthersService
    {
        public bool AddRate(int UserID, int Rate);
    }
}
