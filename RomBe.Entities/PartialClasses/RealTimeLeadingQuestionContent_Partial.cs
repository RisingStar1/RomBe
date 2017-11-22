using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
    [MetadataType(typeof(RealTimeLeadingQuestionContentMetaData))]
    public partial class RealTimeLeadingQuestionContent
    {

        public class RealTimeLeadingQuestionContentMetaData
        {
            [StringLength(565, ErrorMessage = "Question cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Question Is Required Field")]
            public string LeadingQuestion { get; set; }
            public string Subject { get; set; }
            
            [StringLength(565, ErrorMessage = "No Answer cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "No Answer Is Required Field")]
            public string TextForNoAnswer { get; set; }

            [StringLength(565, ErrorMessage = "Yes Answer cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Yes Answer Is Required Field")]
            public string TextForYesAnswer { get; set; }

        }
    }
}
