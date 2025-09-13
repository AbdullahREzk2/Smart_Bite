using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class VerifyOTPDTO
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
    }
}
