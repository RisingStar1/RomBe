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
    
    public partial class UserDevice
    {
        public string DeviceId { get; set; }
        public int UserId { get; set; }
        public System.DateTime InsertDate { get; set; }
        public string DeviceName { get; set; }
        public int OsType { get; set; }
        public string PushToken { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string OsVersion { get; set; }
    
        public virtual User User { get; set; }
    }
}
