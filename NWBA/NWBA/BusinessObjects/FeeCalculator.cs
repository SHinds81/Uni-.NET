using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA.BusinessObjects
{
    public class FeeCalculator
    {
        double Fee { get; set; }

        bool FeeAppicable { get; set; }

        public double GetFee()
        {
            return Fee;
        }

        public bool IsFeeNeeded(int AccountNumber)
        {
            return FeeAppicable;
        }

    }
}
