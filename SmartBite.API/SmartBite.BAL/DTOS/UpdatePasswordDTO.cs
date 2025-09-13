using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class UpdatePasswordDTO
    {
        public string? Email {  get; set; }
        public string? Password { get; set; }
    }
}
