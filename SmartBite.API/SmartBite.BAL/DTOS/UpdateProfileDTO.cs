using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class UpdateProfileDTO
    {
       public int UserID {  get; set; }
       public UserHealthprofileDataDTO? Data { get; set; }
    }
}
