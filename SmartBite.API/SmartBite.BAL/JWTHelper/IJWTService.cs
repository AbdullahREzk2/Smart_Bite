using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SmartBite.BAL.JWTHelper
{
    public interface IJWTService
    {
        public string GenerateToken(IEnumerable<Claim> claims);
    }
}
