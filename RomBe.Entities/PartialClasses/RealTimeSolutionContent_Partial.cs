using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace RomBe.Entities
{
     [MetadataType(typeof(RealTimeSolutionContentMetaData))]
    public partial class RealTimeSolutionContent
    {
        
         public class RealTimeSolutionContentMetaData
        {
            [StringLength(565, ErrorMessage = "Solution cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Solution Is Required Field")]
             
             public string SolutionContent { get; set; }
            
        }
    }
}
