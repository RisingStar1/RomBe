using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public class CalculationHelper
    {
        public static int CalculateAge(DateTime birthDate)
        {
            return (int)((DateTime.Now - birthDate).TotalDays / 365.242199);
        }

    }
}
