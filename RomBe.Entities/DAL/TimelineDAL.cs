using RomBe.Entities.Class.Request;
using RomBe.Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System;
using RomBe.Helpers;
using RomBe.Entities.Class.Timeline;

namespace RomBe.Entities.DAL
{
    public class TimelineDAL
    {
        public async Task<ChildActivity> GetActiviy(int childId, int taskId, TaskTypeEnum taskType)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return await (from activity in context.ChildActivities
                              where activity.ChildId == childId &&
                              activity.TaskTypeId == (int)taskType &&
                              activity.TaskId==taskId
                              select activity).FirstOrDefaultAsync().ConfigureAwait(false);

            }
        }
        public async Task<List<ChildActivity>> GetRealTimeActivitiesIdsAsync(int childId)
        {
            List<ChildActivity> _temporaryList = new List<ChildActivity>();
            using (RombeEntities context = new RombeEntities())
            {
                return await (from activity in context.ChildActivities
                              join rt in context.RealTimeLeadingQuestions on activity.TaskId equals rt.RealTimeLeadingQuestionId
                              where activity.ChildId == childId &&
                              activity.TaskTypeId == (int)TaskTypeEnum.RealTime
                              //&& activity.TaskStatusId != (int)TaskStatusEnum.RT_LeadingQuestionHasNotHappenedYet
                              //&&  (rt.PeriodMin >= request.MinWeeks &&
                              //rt.PeriodMax <= request.MaxWeeks)

                              select activity).ToListAsync().ConfigureAwait(false);

            }
        }

        public async Task<List<ChildActivity>> GetProActiveChildActivitiesIdsAsync(int childId)
        {
            List<ChildActivity> _temporaryList = new List<ChildActivity>();
            using (RombeEntities context = new RombeEntities())
            {


                return await (from activity in context.ChildActivities
                              join pa in context.ProActiveInformations on activity.TaskId equals pa.ProActiveInformationId
                              where activity.ChildId == childId &&
                              activity.TaskTypeId != (int)TaskTypeEnum.RealTime
                              //&& (pa.PeriodMin >= request.MinWeeks &&
                              // pa.PeriodMax <= request.MaxWeeks)
                              select activity).ToListAsync().ConfigureAwait(false);

            }
        }

        public List<ChildActivity> GetRealTimeActivitiesIds(int childId)
        {
            List<ChildActivity> _temporaryList = new List<ChildActivity>();
            using (RombeEntities context = new RombeEntities())
            {
                return (from activity in context.ChildActivities
                        join rt in context.RealTimeLeadingQuestions on activity.TaskId equals rt.RealTimeLeadingQuestionId
                        where activity.ChildId == childId &&
                        activity.TaskTypeId == (int)TaskTypeEnum.RealTime
                        //&& activity.TaskStatusId != (int)TaskStatusEnum.RT_LeadingQuestionHasNotHappenedYet
                        //&&  (rt.PeriodMin >= request.MinWeeks &&
                        //rt.PeriodMax <= request.MaxWeeks)

                        select activity).ToList();

            }
        }

        public List<ChildActivity> GetProActiveChildActivitiesIds(int childId)
        {
            List<ChildActivity> _temporaryList = new List<ChildActivity>();
            using (RombeEntities context = new RombeEntities())
            {


                return (from activity in context.ChildActivities
                        join pa in context.ProActiveInformations on activity.TaskId equals pa.ProActiveInformationId
                        where activity.ChildId == childId &&
                        activity.TaskTypeId != (int)TaskTypeEnum.RealTime
                        //&& (pa.PeriodMin >= request.MinWeeks &&
                        // pa.PeriodMax <= request.MaxWeeks)
                        select activity).ToList();

            }
        }

        public List<RealTimeLeadingQuestion> GetRealTimeTasksList()
        {

            using (RombeEntities context = new RombeEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;

                return (from realTimeLeadingQuestions in context.RealTimeLeadingQuestions
                           .Include(a => a.RealTimeLeadingQuestionContents)
                           .Include(a => a.RealTimeSymptomsCongratulations)
                           .Include(a => a.RealTimeSolutions.Select(b => b.RealTimeSolutionContents))
                           .Include(a => a.RealTimeSymptoms.Select(b => b.RealTimeSymptomsContents))
                           .Include(a => a.TaskCategory)
                        where realTimeLeadingQuestions.IsActive
                        orderby realTimeLeadingQuestions.PeriodMin
                        select realTimeLeadingQuestions).ToList();
            }
        }

        public List<int> GetRealTimeSolutionActivitiesIds(int childId, int taskId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return (from rtsa in context.ChildRealTimeSolutionActivities
                        where rtsa.ChildId == childId && rtsa.RealTimeLeadingQuestionId == taskId
                        orderby rtsa.InsertDate descending
                        select rtsa.RealTimeSolutionId).ToList<int>();
            }
        }

        public List<int> GetRealTimeSymptomsActivitiesIds(int childId, int taskId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return (from rtsa in context.ChildRealTimeSymptomsActivities
                        where rtsa.ChildId == childId && rtsa.RealTimeLeadingQuestionId == taskId
                        orderby rtsa.InsertDate descending
                        select rtsa.RealTimeSymptomsId).ToList<int>();
            }
        }

        public List<ProActiveInformation> GetProActiveTasksList()
        {
            using (RombeEntities context = new RombeEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                return (from pa in context.ProActiveInformations.Include(a => a.ProActiveInformationContents)
                        where pa.IsActive
                        orderby pa.PeriodMax
                        select pa)
                        .ToList();




            }
        }

        public bool UpdateProActiveTask(UpdateProActiveTaskRequest request)
        {
            using (RombeEntities context = new RombeEntities())
            {
                ChildActivity _newActivity = new ChildActivity()
                {
                    ChildId = request.ChildId,
                    TaskStatusId = (int)request.TaskStatus,
                    TaskTypeId = (int)request.TaskType,
                    TaskId = request.TaskId,
                    InsertDate = DateTime.Now,
                    UpdateDate = DateTime.Now

                };

                context.ChildActivities.Add(_newActivity);
                return Convert.ToBoolean(context.SaveChanges());

            }
        }

        public bool UpdateRealTimeTaskQuestion(UpdateRealTimeTaskQuestionRequest request)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    CreateBaseActivityObject _baseActivity = GetBaseActivity(request.ChildId, request.TaskId, request.TaskStatus);

                    UpdateBaseActivityAndCreateActivityLog(_baseActivity, (int)request.TaskStatus);

                    return Convert.ToBoolean(context.SaveChanges());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateRealTimeTaskSymptom(UpdateRealTimeTaskSymptomRequest request)
        {
            using (RombeEntities context = new RombeEntities())
            {
                CreateBaseActivityObject _baseActivity = GetBaseActivity(request.ChildId, request.TaskId, request.TaskStatus);

                foreach (int id in request.SymptomsIds)
                {
                    bool isExist = context.ChildRealTimeSymptomsActivities.Where(s => s.ChildId == request.ChildId &&
                    s.RealTimeLeadingQuestionId == request.TaskId &&
                      s.RealTimeSymptomsId == id).Any();

                    if (!isExist)
                    {
                        ChildRealTimeSymptomsActivity _newSymptomsActivity = new ChildRealTimeSymptomsActivity()
                        {
                            ChildId = request.ChildId,
                            RealTimeLeadingQuestionId = request.TaskId,
                            RealTimeSymptomsId = id,
                            InsertDate = DateTime.Now
                        };
                        context.ChildRealTimeSymptomsActivities.Add(_newSymptomsActivity);
                    }
                }

                UpdateBaseActivityAndCreateActivityLog(_baseActivity, (int)request.TaskStatus);

                return Convert.ToBoolean(context.SaveChanges());
            }
        }

        public bool UpdateRealTimeTaskSolution(UpdateRealTimeTaskSolutionRequest request)
        {
            using (RombeEntities context = new RombeEntities())
            {
                CreateBaseActivityObject _baseActivity = GetBaseActivity(request.ChildId, request.TaskId, request.TaskStatus);

                foreach (int id in request.SolutionIds)
                {
                    bool isExist = context.ChildRealTimeSolutionActivities.Where(s => s.ChildId == request.ChildId &&
                    s.RealTimeLeadingQuestionId == request.TaskId &&
                      s.RealTimeSolutionId == id).Any();

                    if (!isExist)
                    {
                        ChildRealTimeSolutionActivity _newSolutionActivity = new ChildRealTimeSolutionActivity()
                        {
                            ChildId = request.ChildId,
                            RealTimeLeadingQuestionId = request.TaskId,
                            RealTimeSolutionId = id,
                            InsertDate = DateTime.Now
                        };
                        context.ChildRealTimeSolutionActivities.Add(_newSolutionActivity);
                    }
                }

                UpdateBaseActivityAndCreateActivityLog(_baseActivity, (int)request.TaskStatus);
                return Convert.ToBoolean(context.SaveChanges());
            }
        }

        private void CreateChildActivityHistory(int childId, int taskStatus, int taskId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    bool isExist = context.ChildActivitiesHistories.Where(a => a.ChildId == childId && a.TaskId == taskId &&
                      a.TaskStatusId == taskStatus && a.TaskTypeId == (int)TaskTypeEnum.RealTime).Any();
                    if (!isExist)
                    {
                        ChildActivitiesHistory _newActivityHistory = new ChildActivitiesHistory()
                        {
                            ChildId = childId,
                            TaskId = taskId,
                            TaskStatusId = taskStatus,
                            TaskTypeId = (int)TaskTypeEnum.RealTime,
                            InsertDate = DateTime.Now
                        };
                        context.ChildActivitiesHistories.Add(_newActivityHistory);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private CreateBaseActivityObject GetBaseActivity(int childId, int taskId, TaskStatusEnum taskStatus)
        {
            using (RombeEntities context = new RombeEntities())
            {
                ChildActivity _baseActivity = context.ChildActivities.Where(c => c.ChildId == childId && c.TaskId == taskId &&
                                                c.TaskTypeId == (int)TaskTypeEnum.RealTime).FirstOrDefault();
                if (_baseActivity.IsNull())
                {
                    ChildActivity _newBaseActivity = new ChildActivity()
                    {
                        ChildId = childId,
                        TaskTypeId = (int)TaskTypeEnum.RealTime,
                        TaskId = taskId,
                        InsertDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        TaskStatusId = (int)taskStatus
                    };
                    context.ChildActivities.Add(_newBaseActivity);
                    context.SaveChanges();
                    return new CreateBaseActivityObject()
                    {
                        IsCreatedNow = true,
                        Activity = _newBaseActivity
                    };
                }
                return new CreateBaseActivityObject()
                {
                    IsCreatedNow = false,
                    Activity = _baseActivity
                };
            }
        }

        private void UpdateBaseActivityAndCreateActivityLog(CreateBaseActivityObject baseActivity, int taskStatusId)
        {
            if (!baseActivity.IsCreatedNow)
            {
                CreateChildActivityHistory(baseActivity.Activity.ChildId, baseActivity.Activity.TaskStatusId, baseActivity.Activity.TaskId);

                baseActivity.Activity.UpdateDate = DateTime.Now;
                baseActivity.Activity.TaskStatusId = taskStatusId;
            }
        }
    }

    public class CreateBaseActivityObject
    {
        public bool IsCreatedNow { get; set; }
        public ChildActivity Activity { get; set; }
    }
}

