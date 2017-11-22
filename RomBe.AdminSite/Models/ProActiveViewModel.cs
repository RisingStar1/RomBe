using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RomBe.Entities;
using System.Data.Entity;

namespace RomBe.Services.Models
{
    public class ProActiveViewModel
    {
        public IEnumerable<ProActiveInformation> ProActiveInformation { get; set; }
        public IEnumerable<ProActiveInformationContent> ProActiveInformationContent { get; set; }

        public ProActiveViewModel(String subjectSearch, String otherSearch)
        {
            RombeEntities db = new RombeEntities();
            if (!String.IsNullOrEmpty(subjectSearch))
            {
                ProActiveInformation = db.ProActiveInformations.Include(p => p.TaskCategory)
                    .Where(p => p.Subject.ToLower().Contains(subjectSearch.ToLower()))
                    .OrderByDescending(p => p.UpdateDate);

                ProActiveInformationContent = db.ProActiveInformationContents.Include(p => p.Language)
                    .Where(p => ProActiveInformation.Any(a => a.ProActiveInformationId == p.ProActiveInformationId))
                    .OrderByDescending(p => p.UpdateDate);

                return;

            }
            else if (!String.IsNullOrEmpty(otherSearch))
            {
                ProActiveInformationContent = db.ProActiveInformationContents.Include(p => p.Language)
                    .Where(p => p.Information.ToLower().Contains(otherSearch.ToLower()) || p.Title.ToLower().Contains(otherSearch.ToLower()))
                    .OrderByDescending(p => p.UpdateDate);

                ProActiveInformation = db.ProActiveInformations.Include(p => p.TaskCategory)
                    .Where(p => ProActiveInformationContent.Any(a => a.ProActiveInformationId == p.ProActiveInformationId))
                    .OrderByDescending(p => p.UpdateDate);

                return;
            }

            ProActiveInformation = db.ProActiveInformations.Include(p => p.TaskCategory).OrderByDescending(p => p.UpdateDate);
            ProActiveInformationContent = db.ProActiveInformationContents.Include(p => p.Language).OrderByDescending(p => p.UpdateDate);


        }
    }
}