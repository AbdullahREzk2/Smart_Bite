using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SmartBite.DAL
{
    public class dbHelper
    {
        private readonly string? _configuration;


        public dbHelper(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        public string? GetconnectionString()
        {
            return _configuration;
        }



    }
}
