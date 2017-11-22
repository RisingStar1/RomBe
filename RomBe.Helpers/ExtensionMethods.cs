using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsNull<T>(this T input)
        {
            var output = false;
            if (null == input)
            {
                output = true;
            }
            if (input !=null && input.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty(input.ToString()))
                {
                    output = true;
                }
            }
            
            return output;
        }

    }
}
