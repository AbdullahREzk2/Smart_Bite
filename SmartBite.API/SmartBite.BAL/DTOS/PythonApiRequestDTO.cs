using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBite.BAL.DTOS
{
    public class PythonApiRequestDTO
    {

        public int RIAGENDR { get; set; }
        public float RIDAGEYR { get; set; }
        public float BMXWT { get; set; }
        public float BMXHT { get; set; }
        public float BMXBMI { get; set; }
        public int BPQ020 { get; set; }
        public int BPQ050A { get; set; }
        public int DIQ010 { get; set; }
        public int obesity { get; set; }
        public int DIQ050 { get; set; }
        public int MCQ300A { get; set; }
        public int activity_level { get; set; }
        public float BMR { get; set; }
    }

}
