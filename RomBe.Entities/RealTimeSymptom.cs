//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RomBe.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RealTimeSymptom
    {
        public RealTimeSymptom()
        {
            this.ChildRealTimeSymptomsActivities = new HashSet<ChildRealTimeSymptomsActivity>();
            this.RealTimeSymptomsContents = new HashSet<RealTimeSymptomsContent>();
        }
    
        public int RealTimeSymptomsId { get; set; }
        public int RealTimeLeadingQuestionId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime InsertDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual ICollection<ChildRealTimeSymptomsActivity> ChildRealTimeSymptomsActivities { get; set; }
        public virtual RealTimeLeadingQuestion RealTimeLeadingQuestion { get; set; }
        public virtual ICollection<RealTimeSymptomsContent> RealTimeSymptomsContents { get; set; }
    }
}
