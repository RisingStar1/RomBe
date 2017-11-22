using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
     [MetadataType(typeof(ProActiveInformationMetaData))]
    public partial class ProActiveInformation
    {
       
        public class ProActiveInformationMetaData
        {
            [StringLength(100, ErrorMessage = "Subject cannot be longer than 100 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Required Field")]
            public string Subject { get; set; }
            
            [Range(0, 200, ErrorMessage = "0 Is Minimum")]
            public int PeriodMin { get; set; }
           
            [Range(0, 200, ErrorMessage = "0 Is Minimum")]
            public int PeriodMax { get; set; }
        }
    }
}
