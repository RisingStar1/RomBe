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
    
    public partial class Child
    {
        public Child()
        {
            this.ChildActivitiesHistories = new HashSet<ChildActivitiesHistory>();
            this.ChildActivities = new HashSet<ChildActivity>();
            this.ChildRealTimeSolutionActivities = new HashSet<ChildRealTimeSolutionActivity>();
            this.ChildRealTimeSymptomsActivities = new HashSet<ChildRealTimeSymptomsActivity>();
            this.NotificationLogs = new HashSet<NotificationLog>();
        }
    
        public int UserId { get; set; }
        public int ChildId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public System.DateTime InsertDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Birth Birth { get; set; }
        public virtual ICollection<ChildActivitiesHistory> ChildActivitiesHistories { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ChildActivity> ChildActivities { get; set; }
        public virtual ICollection<ChildRealTimeSolutionActivity> ChildRealTimeSolutionActivities { get; set; }
        public virtual ICollection<ChildRealTimeSymptomsActivity> ChildRealTimeSymptomsActivities { get; set; }
        public virtual ICollection<NotificationLog> NotificationLogs { get; set; }
    }
}
