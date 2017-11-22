using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
     [MetadataType(typeof(RealTimeLeadingQuestionMetaData))]
    public partial class RealTimeLeadingQuestion
    {

         public class RealTimeLeadingQuestionMetaData
        {
            [Range(0, 200, ErrorMessage = "0 Is Minimum")]
            public int PeriodMin { get; set; }
            
             [Range(0, 200, ErrorMessage = "0 Is Minimum")]
            public int PeriodMax { get; set; }
        }
    }
}
