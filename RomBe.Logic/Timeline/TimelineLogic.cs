using RomBe.Entities;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Timeline;
using RomBe.Entities.DAL;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RomBe.Helpers;
using System.Collections.Concurrent;
using System.Diagnostics;
using MoreLinq;

namespace RomBe.Logic.Timeline
{
    public class TimelineLogic
    {

        private const int PAGE_SIZE = 20;

        private HashSet<ProActiveObject> _proActiveTasksList;
        private HashSet<RealTimeObject> _realTimeTaskList;


        public HashSet<ProActiveObject> ProActiveTasksList
        {
            get
            {
                if (_proActiveTasksList.IsNull())
                {
                    _proActiveTasksList = GetProActiveTasksList();
                }
                return _proActiveTasksList;
            }
            set { _proActiveTasksList = value; }
        }
        public HashSet<RealTimeObject> RealTimeTaskList
        {
            get
            {
                if (_realTimeTaskList.IsNull())
                {
                    _realTimeTaskList = GetRealTimeTasksList();
                }
                return _realTimeTaskList;
            }
            set { _realTimeTaskList = value; }
        }

        public async Task<GetTimelineDateResponse> GetNextWeekTimelineContent(GetTimelineContentPaginationRequest request)
        {
            return await GetTimelineContentAsync(request, true);

        }
        public async Task<GetTimelineDateResponse> GetPreviousWeekTimelineContent(GetTimelineContentPaginationRequest request)
        {

            GetTimelineDateResponse response = await GetTimelineContentAsync(request, false);
            response.NextPage = null;
            return response;

        }
        public async Task<GetTimelineDateResponse> InitTimeline(GetTimelineContentPaginationRequest request, bool? isNextWeekRequest = null)
        {
            if (SetMinAndMaxWeeks(request))
            {
                return await GetTimelineContentAsync(request, isNextWeekRequest);
            }
            return new GetTimelineDateResponse()
            {
                HttpStatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        public GetTimelineDateResponse InitTimelineWithOutValidation(GetTimelineContentPaginationRequest request)
        {

            return GetTimelineContent(request);

        }
        public UpdateTaskResponse UpdateProActiveTask(UpdateProActiveTaskRequest request)
        {
            UpdateTaskResponse _response = new UpdateTaskResponse();
            bool isSucceedToUpdate = new TimelineDAL().UpdateProActiveTask(request);
            if (isSucceedToUpdate)
            {
                ProActiveObject _currentTask = ProActiveTasksList.Where(p => p.Id == request.TaskId).FirstOrDefault();
                _currentTask.IsDone = true;
                _currentTask.TaskStatus = request.TaskStatus;
                _response.UpdatedTask = _currentTask;
            }
            else
            {
                LoggerHelper.Info(request);
                _response.HttpStatusCode = System.Net.HttpStatusCode.NotModified;
            }
            return _response;

        }
        public UpdateTaskResponse UpdateRealTimeQuestionTask(UpdateRealTimeTaskQuestionRequest request)
        {
            UpdateTaskResponse _response = new UpdateTaskResponse();
            bool isSucceedToUpdate = false;

            isSucceedToUpdate = new TimelineDAL().UpdateRealTimeTaskQuestion(request);

            if (isSucceedToUpdate)
            {
                CommonUpdateRealTimeTask(request, _response);
            }
            else
            {
                LoggerHelper.Info(request);
                _response.HttpStatusCode = System.Net.HttpStatusCode.NotModified;
            }
            return _response;
        }
        public UpdateTaskResponse UpdateRealTimeSolutionTask(UpdateRealTimeTaskSolutionRequest request)
        {
            UpdateTaskResponse _response = new UpdateTaskResponse();
            bool isSucceedToUpdate = false;

            isSucceedToUpdate = new TimelineDAL().UpdateRealTimeTaskSolution(request);

            if (isSucceedToUpdate)
            {
                CommonUpdateRealTimeTask(request, _response);
            }
            else
            {
                LoggerHelper.Info(request);
                _response.HttpStatusCode = System.Net.HttpStatusCode.NotModified;
            }
            return _response;
        }


        public UpdateTaskResponse UpdateRealTimeSymptomTask(UpdateRealTimeTaskSymptomRequest request)
        {
            UpdateTaskResponse _response = new UpdateTaskResponse();
            bool isSucceedToUpdate = false;

            isSucceedToUpdate = new TimelineDAL().UpdateRealTimeTaskSymptom(request);

            if (isSucceedToUpdate)
            {
                CommonUpdateRealTimeTask(request, _response);
            }
            else
            {
                LoggerHelper.Info(request);
                _response.HttpStatusCode = System.Net.HttpStatusCode.NotModified;
            }
            return _response;
        }
        private void CommonUpdateRealTimeTask(BaseUpdateTaskRequest request, UpdateTaskResponse _response)
        {
            RealTimeObject _currentTask = RealTimeTaskList.Where(p => p.Id == request.TaskId).FirstOrDefault();

            MarkContentInRealTime(_currentTask, request.ChildId);
            _currentTask.TaskStatus = request.TaskStatus;
            _response.UpdatedTask = _currentTask;
        }
        public async Task<object> GetTimelineItem(int childId, int taskId, TaskTypeEnum taskType)
        {
            try
            {
                TimelineDAL _timelineDAL = new TimelineDAL();
                ChildActivity _activity = await _timelineDAL.GetActiviy(childId, taskId, taskType);
                if (taskType == TaskTypeEnum.RealTime)
                {
                    RealTimeObject _tempRealTime = RealTimeTaskList.Where(r => r.Id == taskId).FirstOrDefault();
                    if (!_activity.IsNull())
                    {
                        _tempRealTime.TaskStatus = (TaskStatusEnum)_activity.TaskStatusId;
                    }

                    MarkContentInRealTime(_tempRealTime, childId);
                    return _tempRealTime;
                }
                else
                {
                    ProActiveObject _tempProActive = ProActiveTasksList.Where(p => p.Id == taskId).FirstOrDefault();
                    if (!_activity.IsNull())
                    {
                        _tempProActive.TaskStatus = (TaskStatusEnum)_activity.TaskStatusId;
                        _tempProActive.IsDone = true;
                    }
                    return _tempProActive;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public HashSet<ProActiveObject> SetProActiveTasksListInMemoryCache()
        {
            HashSet<ProActiveObject> proActiveObjectList;
            if (!RedisCacherHelper.Exists(SystemConfigurationHelper.TimelineProActiveTasksCahceKey))
            {
                List<ProActiveInformation> proActiveDataFromDBObjectList = new List<ProActiveInformation>();
                proActiveDataFromDBObjectList = new TimelineDAL().GetProActiveTasksList();
                proActiveObjectList = new HashSet<ProActiveObject>();
                foreach (ProActiveInformation item in proActiveDataFromDBObjectList)
                {
                    proActiveObjectList.Add(BuildBaseProActiveObject(item));
                }

                bool isAddedSucceeded = RedisCacherHelper.Add(SystemConfigurationHelper.TimelineProActiveTasksCahceKey, proActiveObjectList, new DateTimeOffset(DateTime.Now.AddDays(SystemConfigurationHelper.CacheExpirationInDays)));
                return proActiveObjectList;
            }
            else
            {
                return RedisCacherHelper.Get<HashSet<ProActiveObject>>(SystemConfigurationHelper.TimelineProActiveTasksCahceKey);
            }

        }
        public HashSet<RealTimeObject> SetRealTimeTasksListInMemoryCache()
        {
            HashSet<RealTimeObject> realTimeObjectList;
            if (!RedisCacherHelper.Exists(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey))
            {
                realTimeObjectList = new HashSet<RealTimeObject>();
                List<RealTimeLeadingQuestion> realTimeDataFromDBObjectList = new List<RealTimeLeadingQuestion>();
                realTimeDataFromDBObjectList = new TimelineDAL().GetRealTimeTasksList();

                foreach (RealTimeLeadingQuestion item in realTimeDataFromDBObjectList)
                {
                    realTimeObjectList.Add(BuildBaseRealTimeObject(item));
                }


                bool isAddedSucceeded = RedisCacherHelper.Add(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey, realTimeObjectList, new DateTimeOffset(DateTime.Now.AddDays(SystemConfigurationHelper.CacheExpirationInDays)));
                return realTimeObjectList;
            }
            else
            {
                return RedisCacherHelper.Get<HashSet<RealTimeObject>>(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey);
            }
        }

        #region private methods

        private GetTimelineDateResponse GetTimelineContent(GetTimelineContentPaginationRequest request, bool? isNextWeekRequest = null)
        {
            GetTimelineDateResponse _response = new GetTimelineDateResponse()
            {
                ChildId = request.ChildId
            };

            List<ChildActivity> _proActiveActivitesListDbObject = new TimelineDAL().GetProActiveChildActivitiesIds(request.ChildId);
            List<ChildActivity> _realTimeActivitesListDbObject = new TimelineDAL().GetRealTimeActivitiesIds(request.ChildId);


            int numberOfMissingItems = 0;

            //add the activites to the timeline
            for (int weekNumber = request.MinWeeks.Value; weekNumber <= request.MaxWeeks.Value; weekNumber++)
            {
                WeekObject _newWeek = new WeekObject()
                {
                    WeekNumber = weekNumber
                };


                //add the real time in timeline
                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                if (numberOfMissingItems != 0)
                {
                    List<RealTimeObject> _realTimeListForTheCurrentWeek = RealTimeTaskList.Where(r => r.PeriodMax == weekNumber).TakeLast(numberOfMissingItems).ToList();
                    if (!_realTimeActivitesListDbObject.IsNull() && _realTimeActivitesListDbObject.Any())
                    {
                        SetRealTimeActivitesList(_realTimeActivitesListDbObject, request, _realTimeListForTheCurrentWeek);
                    }
                    _newWeek.WeekItems.AddRange(_realTimeListForTheCurrentWeek);
                    foreach (var item in _realTimeListForTheCurrentWeek)
                    {
                        RealTimeTaskList.Remove(item);
                    }
                }

                //set pro active in timeline
                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);

                if (numberOfMissingItems != 0)
                {
                    List<ProActiveObject> _proActiveListForTheCurrentWeek = ProActiveTasksList.Where(p => p.PeriodMax == weekNumber).TakeLast(numberOfMissingItems).ToList();
                    if (!_proActiveActivitesListDbObject.IsNull() && _proActiveActivitesListDbObject.Any())
                    {
                        SetProActiveActivitesList(_proActiveActivitesListDbObject, _proActiveListForTheCurrentWeek);
                    }
                    _newWeek.WeekItems.AddRange(_proActiveListForTheCurrentWeek);
                    foreach (var item in _proActiveListForTheCurrentWeek)
                    {
                        ProActiveTasksList.Remove(item);
                    }

                }


                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                if (numberOfMissingItems != 0)
                {


                    List<RealTimeObject> _realTimeMisingItemsList = RealTimeTaskList.Where(a => a.PeriodMin <= weekNumber
                                                                && a.PeriodMax >= weekNumber).TakeLast(numberOfMissingItems).ToList();

                    if (!_realTimeActivitesListDbObject.IsNull())
                    {
                        SetRealTimeActivitesList(_realTimeActivitesListDbObject, request, _realTimeMisingItemsList);
                    }
                    _newWeek.WeekItems.AddRange(_realTimeMisingItemsList);

                    foreach (RealTimeObject item in _realTimeMisingItemsList)
                    {
                        RealTimeTaskList.Remove(item);
                    }

                    numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                    if (numberOfMissingItems != 0)
                    {
                        List<ProActiveObject> _proActiveMisingItemsList = ProActiveTasksList.Where(a => a.PeriodMin <= weekNumber
                                                                && a.PeriodMax >= weekNumber)
                                                                .TakeLast(numberOfMissingItems).ToList();
                        if (!_proActiveActivitesListDbObject.IsNull())
                        {
                            SetProActiveActivitesList(_proActiveActivitesListDbObject, _proActiveMisingItemsList);
                        }
                        _newWeek.WeekItems.AddRange(_proActiveMisingItemsList);

                        foreach (ProActiveObject item in _proActiveMisingItemsList)
                        {
                            ProActiveTasksList.Remove(item);
                        }
                    }
                }

                _response.TimelineContent.Add(_newWeek);
            }


            //_PreviousPage
            TasksIdToSkipObject _tasksIdToSkipPreviousObject = SetTasksIdToSkipForPreviousWeek(_response.TimelineContent.First());

            //_NextPage
            TasksIdToSkipObject _tasksIdToSkipNextObject = SetTasksIdToSkipForNextWeeks(_response.TimelineContent.Last());


            _response.PreviousPage = BuildPreviousWeekContentUrl(request, _tasksIdToSkipPreviousObject);
            _response.NextPage = BuildNextWeekContentUrl(request, _tasksIdToSkipNextObject);

            return _response;

        }
        private async Task<GetTimelineDateResponse> GetTimelineContentAsync(GetTimelineContentPaginationRequest request, bool? isNextWeekRequest = null)
        {
            GetTimelineDateResponse _response = new GetTimelineDateResponse()
            {
                ChildId = request.ChildId
            };

            SetTasksLists(request, isNextWeekRequest);
            int? _userId = new AuthenticationHelper().GetCurrentUserId();
            if (!_userId.HasValue)
            {
                return new GetTimelineDateResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            User _currentUser = new UserDAL().GetUser(_userId.Value);

            if (_currentUser.IsNull())
            {
                return new GetTimelineDateResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }


            
            List<ChildActivity> _proActiveActivitesListDbObject = await new TimelineDAL().GetProActiveChildActivitiesIdsAsync(request.ChildId);
            List<ChildActivity> _realTimeActivitesListDbObject = await new TimelineDAL().GetRealTimeActivitiesIdsAsync(request.ChildId);
            TasksIdToSkipObject _tasksIdToSkipPreviousObject = null;
            TasksIdToSkipObject _tasksIdToSkipNextObject = null;
            int numberOfMissingItems = 0;

            //add the activites to the timeline
            for (int weekNumber = request.MinWeeks.Value; weekNumber <= request.MaxWeeks.Value; weekNumber++)
            {
                WeekObject _newWeek = new WeekObject()
                {
                    WeekNumber = weekNumber
                };


                //add the real time in timeline
                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                if (numberOfMissingItems != 0)
                {
                    List<RealTimeObject> _realTimeListForTheCurrentWeek = RealTimeTaskList.Where(r => r.PeriodMax == weekNumber).TakeLast(numberOfMissingItems).ToList();
                    if (!_realTimeActivitesListDbObject.IsNull() && _realTimeActivitesListDbObject.Any())
                    {
                        SetRealTimeActivitesList(_realTimeActivitesListDbObject, request, _realTimeListForTheCurrentWeek);
                    }
                    _newWeek.WeekItems.AddRange(_realTimeListForTheCurrentWeek);
                    foreach (var item in _realTimeListForTheCurrentWeek)
                    {
                        RealTimeTaskList.Remove(item);
                    }
                }

                //set pro active in timeline
                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);

                if (numberOfMissingItems != 0)
                {

                    List<ProActiveObject> _proActiveListForTheCurrentWeek = ProActiveTasksList.Where(p => p.PeriodMax == weekNumber).ToList();
                    if (!_proActiveActivitesListDbObject.IsNull() && _proActiveActivitesListDbObject.Any())
                    {
                        SetProActiveActivitesList(_proActiveActivitesListDbObject, _proActiveListForTheCurrentWeek);
                    }


                    if (_proActiveListForTheCurrentWeek.Count > 20)
                    {

                        List<ProActiveObject> _proActiveActivites = _proActiveListForTheCurrentWeek.Where(p => p.IsDone).ToList();
                        numberOfMissingItems = NumberOfMissingItems(_proActiveActivites.Count);
                        _newWeek.WeekItems.AddRange(_proActiveActivites);
                        if (numberOfMissingItems != 0)
                        {
                            Random tempRandom = new Random(_currentUser.InsertDate.Millisecond);
                            _proActiveListForTheCurrentWeek = _proActiveListForTheCurrentWeek.Where(p => !p.IsDone).OrderBy((item) => tempRandom.Next()).Take(numberOfMissingItems).ToList();
                            _newWeek.WeekItems.AddRange(_proActiveListForTheCurrentWeek);
                        }
                    }
                    else
                    {
                        _newWeek.WeekItems.AddRange(_proActiveListForTheCurrentWeek);
                    }

                    foreach (var item in _proActiveListForTheCurrentWeek)
                    {
                        ProActiveTasksList.Remove(item);
                    }

                }

                numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                if (numberOfMissingItems != 0)
                {


                    List<RealTimeObject> _realTimeMisingItemsList = RealTimeTaskList.Where(a => a.PeriodMin <= weekNumber
                                                                && a.PeriodMax >= weekNumber).TakeLast(numberOfMissingItems).ToList();

                    if (!_realTimeActivitesListDbObject.IsNull())
                    {
                        SetRealTimeActivitesList(_realTimeActivitesListDbObject, request, _realTimeMisingItemsList);
                    }
                    _newWeek.WeekItems.AddRange(_realTimeMisingItemsList);

                    foreach (RealTimeObject item in _realTimeMisingItemsList)
                    {
                        RealTimeTaskList.Remove(item);
                    }

                    numberOfMissingItems = NumberOfMissingItems(_newWeek.WeekItems.Count);
                    if (numberOfMissingItems != 0)
                    {
                        List<ProActiveObject> _proActiveMisingItemsList = ProActiveTasksList.Where(a => a.PeriodMin <= weekNumber
                                                                && a.PeriodMax >= weekNumber)
                                                                .TakeLast(numberOfMissingItems).ToList();
                        if (!_proActiveActivitesListDbObject.IsNull())
                        {
                            SetProActiveActivitesList(_proActiveActivitesListDbObject, _proActiveMisingItemsList);
                        }
                        _newWeek.WeekItems.AddRange(_proActiveMisingItemsList);

                        foreach (ProActiveObject item in _proActiveMisingItemsList)
                        {
                            ProActiveTasksList.Remove(item);
                        }
                    }
                }



                //set the ids to skip BEFORE the mix of items
                //_PreviousPage
                if (_tasksIdToSkipPreviousObject.IsNull())
                {
                    _tasksIdToSkipPreviousObject = SetTasksIdToSkipForPreviousWeek(_newWeek);
                }
                //_NextPage
                if (_tasksIdToSkipNextObject.IsNull() && (weekNumber + 1) > request.MaxWeeks.Value)
                {
                    _tasksIdToSkipNextObject = SetTasksIdToSkipForNextWeeks(_newWeek);
                }

                //mix the items
                Random rnd = new Random(_currentUser.InsertDate.Millisecond);
                _newWeek.WeekItems = _newWeek.WeekItems.OrderBy((item) => rnd.Next()).ToList();

                _response.TimelineContent.Add(_newWeek);
            }


            //_PreviousPage
            //TasksIdToSkipObject _tasksIdToSkipPreviousObject = SetTasksIdToSkipForPreviousWeek(_response.TimelineContent.First());

            //_NextPage
            //TasksIdToSkipObject _tasksIdToSkipNextObject = SetTasksIdToSkipForNextWeeks(_response.TimelineContent.Last());



            _response.PreviousPage = BuildPreviousWeekContentUrl(request, _tasksIdToSkipPreviousObject);
            _response.NextPage = BuildNextWeekContentUrl(request, _tasksIdToSkipNextObject);

            return _response;

        }

        private void SetTasksLists(GetTimelineContentPaginationRequest request, bool? isNextWeekRequest)
        {
            if (isNextWeekRequest.HasValue)
            {
                if (isNextWeekRequest.Value)
                {
                    //for next week 
                    if (request.PaTaskIdToSkip != 0)
                    {
                        ProActiveTasksList = new HashSet<ProActiveObject>(ProActiveTasksList.Where(p => p.UniqueId > request.PaTaskIdToSkip));
                    }
                    if (request.RtTaskIdToSkip != 0)
                    {
                        RealTimeTaskList = new HashSet<RealTimeObject>(RealTimeTaskList.Where(r => r.UniqueId > request.RtTaskIdToSkip));
                    }
                }
                else
                {
                    //for previous week
                    if (request.PaTaskIdToSkip != 0)
                    {
                        ProActiveTasksList = new HashSet<ProActiveObject>(ProActiveTasksList.Where(p => p.UniqueId < request.PaTaskIdToSkip));
                    }
                    if (request.RtTaskIdToSkip != 0)
                    {
                        RealTimeTaskList = new HashSet<RealTimeObject>(RealTimeTaskList.Where(r => r.UniqueId < request.RtTaskIdToSkip));
                    }
                }
            }
        }

        private TasksIdToSkipObject SetTasksIdToSkipForNextWeeks(WeekObject lastWeek)
        {
            TasksIdToSkipObject response = new TasksIdToSkipObject();
            foreach (var item in lastWeek.WeekItems)
            {
                if (item.GetType() == typeof(ProActiveObject))
                {
                    response.ProActiveTaskId = ((ProActiveObject)item).UniqueId;
                }
                else if (item.GetType() == typeof(RealTimeObject))
                {
                    response.RealTimeTaskId = ((RealTimeObject)item).UniqueId;
                }
            }

            return response;
        }
        private TasksIdToSkipObject SetTasksIdToSkipForPreviousWeek(WeekObject firstWeek)
        {
            TasksIdToSkipObject response = new TasksIdToSkipObject();

            foreach (var item in firstWeek.WeekItems)
            {
                if (item.GetType() == typeof(ProActiveObject) && response.ProActiveTaskId == 0)
                {
                    response.ProActiveTaskId = ((ProActiveObject)item).UniqueId;
                }
                else if (item.GetType() == typeof(RealTimeObject) && response.RealTimeTaskId == 0)
                {
                    response.RealTimeTaskId = ((RealTimeObject)item).UniqueId;
                }
            }

            return response;
        }
        private void SetProActiveActivitesList(List<ChildActivity> childActivitiesIds, List<ProActiveObject> proActiveList)
        {

            List<ChildActivity> query = (from activity in childActivitiesIds
                                         join task in proActiveList on activity.TaskId equals task.Id
                                         select activity).ToList();

            foreach (ChildActivity activity in query)
            {
                ProActiveObject temp = proActiveList.FirstOrDefault(r => r.Id == activity.TaskId);
                if (!temp.IsNull())
                {
                    temp.TaskStatus = (TaskStatusEnum)activity.TaskStatusId;
                    temp.IsDone = true;
                }

            }

        }
        private void SetRealTimeActivitesList(List<ChildActivity> childActivitiesIds, BaseTimelineDateRequest request, List<RealTimeObject> realTimeList)
        {

            List<ChildActivity> query = (from activity in childActivitiesIds
                                         join task in realTimeList on activity.TaskId equals task.Id
                                         select activity).ToList();

            foreach (ChildActivity activity in childActivitiesIds)
            {

                RealTimeObject temp = realTimeList.FirstOrDefault(r => r.Id == activity.TaskId);
                if (!temp.IsNull())
                {
                    temp.TaskStatus = (TaskStatusEnum)activity.TaskStatusId;
                    MarkContentInRealTime(temp, request.ChildId);
                }
            }

        }
        private ProActiveObject BuildBaseProActiveObject(ProActiveInformation proActiveDataFromDBObject)
        {
            ProActiveObject _tipObject = new ProActiveObject();
            _tipObject.Id = proActiveDataFromDBObject.ProActiveInformationId;
            _tipObject.Subject = proActiveDataFromDBObject.Subject;
            _tipObject.Body = proActiveDataFromDBObject.ProActiveInformationContents.First().Title;
            _tipObject.Action = proActiveDataFromDBObject.ProActiveInformationContents.First().Information;
            _tipObject.Type = (TaskTypeEnum)proActiveDataFromDBObject.ProActiveTypeId;
            _tipObject.PeriodMin = proActiveDataFromDBObject.PeriodMin;
            _tipObject.PeriodMax = proActiveDataFromDBObject.PeriodMax;
            _tipObject.UniqueId = proActiveDataFromDBObject.UniqueId.Value;
            return _tipObject;
        }
        private RealTimeObject BuildBaseRealTimeObject(RealTimeLeadingQuestion realTimeDataFromDBObject)
        {
            RealTimeObject _realTimeObject = new RealTimeObject();
            _realTimeObject.Type = TaskTypeEnum.RealTime;
            _realTimeObject.Id = realTimeDataFromDBObject.RealTimeLeadingQuestionContents.First().RealTimeLeadingQuestionId;
            _realTimeObject.Subject = realTimeDataFromDBObject.RealTimeLeadingQuestionContents.First().Subject;
            _realTimeObject.Question = realTimeDataFromDBObject.RealTimeLeadingQuestionContents.First().LeadingQuestion;
            _realTimeObject.NegativeAnswer = realTimeDataFromDBObject.RealTimeLeadingQuestionContents.First().TextForNoAnswer;
            _realTimeObject.PositiveAnswer = realTimeDataFromDBObject.RealTimeLeadingQuestionContents.First().TextForYesAnswer;
            _realTimeObject.TaskCategory = realTimeDataFromDBObject.TaskCategory.CategoryName;
            _realTimeObject.PeriodMin = realTimeDataFromDBObject.PeriodMin;
            _realTimeObject.PeriodMax = realTimeDataFromDBObject.PeriodMax;
            _realTimeObject.UniqueId = realTimeDataFromDBObject.UniqueId.GetValueOrDefault();
            _realTimeObject.CompletionMessage = "Great , You are one step ahead now.";
            _realTimeObject.DismissionMessage = "OK , Maybe later when you have more time.";

            foreach (RealTimeSymptom symptom in realTimeDataFromDBObject.RealTimeSymptoms)
            {
                if (symptom.RealTimeSymptomsContents.Any())
                {
                    if (symptom.RealTimeSymptomsContents.First().SymptomsContent != "none")
                    {

                        _realTimeObject.Symptoms.Add(new RealTimeSymptomObject()
                        {
                            Text = symptom.RealTimeSymptomsContents.First().SymptomsContent,
                            Id = symptom.RealTimeSymptomsContents.First().RealTimeSymptomsId
                        });
                    }
                }
            }

            foreach (RealTimeSolution solution in realTimeDataFromDBObject.RealTimeSolutions)
            {
                if (solution.RealTimeSolutionContents.Any())
                {
                    _realTimeObject.Solutions.Add(new RealTimeSolutionObject()
                    {
                        Text = solution.RealTimeSolutionContents.First().SolutionContent,
                        Id = solution.RealTimeSolutionContents.First().RealTimeSolutionId
                    });
                }

            }

            return _realTimeObject;
        }
        public void MarkContentInRealTime(RealTimeObject item, int childId)
        {
            TimelineDAL _timeLineDAL = new TimelineDAL();
            List<int> _realTimeSolutionActivitiesIds = new List<int>();
            List<int> _realTimeSymptomsActivitiesIds = new List<int>();

            _realTimeSolutionActivitiesIds = _timeLineDAL.GetRealTimeSolutionActivitiesIds(childId, item.Id);
            _realTimeSymptomsActivitiesIds = _timeLineDAL.GetRealTimeSymptomsActivitiesIds(childId, item.Id);


            foreach (RealTimeSymptomObject symptom in item.Symptoms)
            {
                symptom.IsSelected = _realTimeSymptomsActivitiesIds.Contains(symptom.Id);
            }

            foreach (RealTimeSolutionObject solution in item.Solutions)
            {
                solution.IsSelected = _realTimeSolutionActivitiesIds.Contains(solution.Id);

            }

            if (item.Solutions.Where(s => s.IsSelected == true).Any())
            {
                item.IsDone = true;
            }

            else if (item.TaskStatus == TaskStatusEnum.RT_ChooseNotToSeeSolutions)
            {
                item.IsDone = true;
            }

        }
        public int? CalculateUserAgeInWeeks(int childId, int userLocalTime)
        {
            DateTime? _childBirthDate = new ChildDAL().GetChildBirthDate(childId);
            if (_childBirthDate.HasValue)
            {
                double temp = Math.Truncate((double)((DateTime.UtcNow.AddHours(userLocalTime) - _childBirthDate.Value).TotalDays / 7));
                return Convert.ToInt32(temp);
            }
            else
            {
                return null;
            }
        }
        private bool SetMinAndMaxWeeks(BaseTimelineDateRequest request)
        {
            int? _userId = new AuthenticationHelper().GetCurrentUserId();
            if (!_userId.HasValue) return false;

            User _currentUser = new UserDAL().GetUser(_userId.Value);
            if (_currentUser.IsNull()) return false;

            int? _childAgeInWeeks = CalculateUserAgeInWeeks(request.ChildId, _currentUser.LocalTime.Value);
            if (!_childAgeInWeeks.HasValue)
            {
                return false;
            }
            request.MinWeeks = _childAgeInWeeks.Value+1;
            request.MaxWeeks = _childAgeInWeeks.Value + 10;
            return true;
        }
        private PagingObejct BuildPreviousWeekContentUrl(BaseTimelineDateRequest request, TasksIdToSkipObject IdsToSkip)
        {
            int _minWeek = request.MinWeeks.Value - 1;
            int _maxWeek = _minWeek;

            if (_minWeek >= 0)
            {

                return new PagingObejct()
                {
                    ChildId = request.ChildId,
                    MaxWeek = _maxWeek,
                    MinWeek = _minWeek,
                    ProActiveTaskIdToSkip = IdsToSkip.ProActiveTaskId,
                    RealTimeTaskIdToSkip = IdsToSkip.RealTimeTaskId
                };

            }
            else
            {
                return null;
            }
        }
        private PagingObejct BuildNextWeekContentUrl(BaseTimelineDateRequest request, TasksIdToSkipObject IdsToSkip)
        {
            int _minWeek = request.MaxWeeks.Value + 1;
            int _maxWeek = _minWeek + 9;

            //if (_minWeek > 52)
            //{
            //    return null;
            //}

            //if (_maxWeek > 52)
            //{
            //    int subtract = _maxWeek - 52;
            //    _maxWeek = _maxWeek - subtract;
            //}

            if (_minWeek > 47)
            {
                return null;
            }

            if (_maxWeek > 47)
            {
                int subtract = _maxWeek - 47;
                _maxWeek = _maxWeek - subtract;
            }
            return new PagingObejct()
            {
                ChildId = request.ChildId,
                MaxWeek = _maxWeek,
                MinWeek = _minWeek,
                ProActiveTaskIdToSkip = IdsToSkip.ProActiveTaskId,
                RealTimeTaskIdToSkip = IdsToSkip.RealTimeTaskId
            };


        }
        private int NumberOfMissingItems(int numberOfItems)
        {
            return PAGE_SIZE - numberOfItems;
        }
        private HashSet<ProActiveObject> GetProActiveTasksList()
        {
            return new HashSet<ProActiveObject>();




            HashSet<ProActiveObject> proActiveInformationList;
            if (RedisCacherHelper.Exists(SystemConfigurationHelper.TimelineProActiveTasksCahceKey))
            {
                proActiveInformationList = RedisCacherHelper.Get<HashSet<ProActiveObject>>(SystemConfigurationHelper.TimelineProActiveTasksCahceKey);
            }
            else
            {
                proActiveInformationList = SetProActiveTasksListInMemoryCache();
            }
            return proActiveInformationList;
        }
        private HashSet<RealTimeObject> GetRealTimeTasksList()
        {



            HashSet<RealTimeObject> realTimeObjectLists;
            
                realTimeObjectLists = new HashSet<RealTimeObject>();
                List<RealTimeLeadingQuestion> realTimeDataFromDBObjectList = new List<RealTimeLeadingQuestion>();
                realTimeDataFromDBObjectList = new TimelineDAL().GetRealTimeTasksList();

                foreach (RealTimeLeadingQuestion item in realTimeDataFromDBObjectList)
                {
                    realTimeObjectLists.Add(BuildBaseRealTimeObject(item));
                }

                return realTimeObjectLists;
           









            HashSet<RealTimeObject> realTimeObjectList;
            if (RedisCacherHelper.Exists(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey))
            {
                realTimeObjectList = RedisCacherHelper.Get<HashSet<RealTimeObject>>(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey);
            }
            else
            {
                realTimeObjectList = SetRealTimeTasksListInMemoryCache();
            }

            return realTimeObjectList;
        }

        #endregion private methods
    }
    class TasksIdToSkipObject
    {
        public int ProActiveTaskId { get; set; }
        public int RealTimeTaskId { get; set; }
    }

}
