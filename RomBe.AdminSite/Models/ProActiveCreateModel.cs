using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RomBe.Entities;
using System.Data.Entity;

namespace RomBe.Services.Models
{
    public class ProActiveCreateModel
    {
        public ProActiveInformation ProActiveInformation { get; set; }
        public ProActiveInformationContent ProActiveInformationContent { get; set; }

        //public ProActiveCreateModel()
        //{
        //    RomBeEntities db = new RomBeEntities();
        //    ProActiveInformation = db.ProActiveInformations;
        //    ProActiveInformationContent = db.ProActiveInformationContents.Include(p => p.Language);

        //}
    }
}