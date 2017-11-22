using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RomBe.Entities;
using System.Data.Entity;

namespace RomBe.Services.Models
{
    public class RealTimeViewModel
    {
        public IEnumerable<RealTimeLeadingQuestion> RealTimeLeadingQuestion { get; set; }
        public IEnumerable<RealTimeLeadingQuestionContent> RealTimeLeadingQuestionContent { get; set; }
        public IEnumerable<RealTimeSymptomsCongratulation> RealTimeSymptomsCongratulation { get; set; }
        public IEnumerable<RealTimeSymptom> RealTimeSymptom { get; set; }
        public IEnumerable<RealTimeSymptomsContent> RealTimeSymptomsContent { get; set; }
        public IEnumerable<RealTimeSolution> RealTimeSolution { get; set; }
        public IEnumerable<RealTimeSolutionContent> RealTimeSolutionContent { get; set; }


        public RealTimeViewModel(String questionString)
        {
            RombeEntities db = new RombeEntities();
            if (String.IsNullOrEmpty(questionString))
            {

                RealTimeLeadingQuestion = db.RealTimeLeadingQuestions.Include(r => r.TaskCategory).ToList();
                RealTimeSymptomsCongratulation = db.RealTimeSymptomsCongratulations.Include(r => r.Language).ToList();
                RealTimeLeadingQuestionContent = db.RealTimeLeadingQuestionContents.Include(r => r.Language).Include(r => r.RealTimeLeadingQuestion).OrderBy(a => a.RealTimeLeadingQuestion.UpdateDate).ToList();
                RealTimeSymptom = db.RealTimeSymptoms.ToList();
                RealTimeSymptomsContent = db.RealTimeSymptomsContents.Include(r => r.Language).ToList();
                RealTimeSolution = db.RealTimeSolutions.ToList();
                RealTimeSolutionContent = db.RealTimeSolutionContents.Include(r => r.Language).ToList();
                return;
            }
            else
            {
                RealTimeLeadingQuestionContent = db.RealTimeLeadingQuestionContents.Include(r => r.Language)
                    .Include(r => r.RealTimeLeadingQuestion)
                    .Where(r => r.LeadingQuestion.ToLower().Contains(questionString.ToLower()));


                RealTimeLeadingQuestion = db.RealTimeLeadingQuestions.Include(r => r.TaskCategory)
                    .Where(p => RealTimeLeadingQuestionContent.Any(a => a.RealTimeLeadingQuestionId == p.RealTimeLeadingQuestionId));

                RealTimeSymptomsCongratulation = db.RealTimeSymptomsCongratulations.Include(r => r.Language)
                    .Where(p => RealTimeLeadingQuestionContent.Any(a => a.RealTimeLeadingQuestionId == p.RealTimeLeadingQuestionId));

                RealTimeSymptom = db.RealTimeSymptoms
                    .Where(p => RealTimeLeadingQuestionContent.Any(a => a.RealTimeLeadingQuestionId == p.RealTimeLeadingQuestionId));

                RealTimeSymptomsContent = db.RealTimeSymptomsContents.Include(r => r.Language)
                    .Where(p => RealTimeSymptom.Any(a => a.RealTimeSymptomsId == p.RealTimeSymptomsId));
                
                RealTimeSolution = db.RealTimeSolutions
                    .Where(p => RealTimeLeadingQuestionContent.Any(a => a.RealTimeLeadingQuestionId == p.RealTimeLeadingQuestionId));

                RealTimeSolutionContent = db.RealTimeSolutionContents.Include(r => r.Language)
                    .Where(p => RealTimeSolution.Any(a => a.RealTimeSolutionId == p.RealTimeSolutionId));

            }

        }

    }
}