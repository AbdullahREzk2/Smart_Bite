using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.Services
{
    public interface IAiPredictionService
    {
        Task<List<string>> PredictLabelsAsync(byte[] imageContent, string fileName, string contentType);
    }

}
