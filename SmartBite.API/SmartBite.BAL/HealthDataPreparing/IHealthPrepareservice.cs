using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartBite.BAL.DTOS;

namespace SmartBite.BAL.HealthDataPreparing
{
    public interface IHealthPrepareservice
    {

        public void CallLimitsModel(int UserID, int hasAllergy, DiseseObjectDTO diseseObject);

        public void CallLimitsModelForUpdate(int UserID, MyAiObjectDTO myAiObject);
    }
}
