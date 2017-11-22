using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RomBe.Entities
{
     [MetadataType(typeof(MonthlyCheckListMetaData))]
    public partial class MonthlyCheckList
    {

         public class MonthlyCheckListMetaData
        {
            [StringLength(49, ErrorMessage = "Title cannot be longer than 50 characters.")]
            [Required(AllowEmptyStrings = false, ErrorMessage = "Required Field")]
            public string Title { get; set; }
            
        }
    }
}
