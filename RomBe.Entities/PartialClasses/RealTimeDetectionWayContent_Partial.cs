using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
     [MetadataType(typeof(RealTimeDetectionWayContentMetaData))]
    public partial class RealTimeDetectionWayContent
    {

         public class RealTimeDetectionWayContentMetaData
        {
            [StringLength(565, ErrorMessage = "Detection Way Content cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Detection Way Content Is Required Field")]
             public string DetectionWayContent { get; set; }
            
        }
    }
}
