using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
     [MetadataType(typeof(RealTimeDetectionWayCongratulationMetaData))]
    public partial class RealTimeDetectionWayCongratulation
    {

         public class RealTimeDetectionWayCongratulationMetaData
        {
            [StringLength(565, ErrorMessage = "Message cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Congratulations Message Is Required Field")]
             public string DetectionWayCongratulationsMessage { get; set; }
            
        }
    }
}
