using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
    [MetadataType(typeof(ProActiveInformationContentMetaData))]
    public partial class ProActiveInformationContent
    {

        public class ProActiveInformationContentMetaData
        {
            [StringLength(565, ErrorMessage = "Title cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
            public string Title { get; set; }
            [StringLength(565, ErrorMessage = "Information cannot be longer than 570 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Required")]
            public string Information { get; set; }

        }
    }
}
