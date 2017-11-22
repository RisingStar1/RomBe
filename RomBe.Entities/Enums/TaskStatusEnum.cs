using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Enums
{
    public enum TaskStatusEnum
    {
        PA_Tip_Yes = 1,
        PA_Tip_No = 2,
        PA_QA_Close = 3,
        RT_LeadingQuestionHasNotHappenedYet = 4,
        RT_LeadingQuestionHasHappened = 5,
        RT_ChooseNotToSeeSolutions = 6,
        RT_ChooseToSeeSolutions = 7,
        RT_SymtopmSelected = 8,
        RT_SolutionSelected = 9,
        General_Ignored = 10
    }
}
