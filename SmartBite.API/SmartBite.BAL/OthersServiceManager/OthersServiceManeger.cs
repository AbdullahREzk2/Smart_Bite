using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.DAL.OthersServices;

namespace SmartBite.BAL.OthersServiceManager
{
    public class OthersServiceManeger : IOthersServiceManeger
    {
        private readonly IOthersService _othersService;

        public OthersServiceManeger(IOthersService othersService)
        {
            _othersService = othersService;
        }


        public bool AddRate(int UserID, int Rate)
        {
            return _othersService.AddRate(UserID, Rate);
        }



    }
}
