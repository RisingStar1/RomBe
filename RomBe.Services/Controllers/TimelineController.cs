using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Timeline;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using RomBe.Logic.Timeline;
using RomBe.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RomBe.Services.Controllers
{
    [Authorize]
    [IsTheChildRelatedToTheUser]
    public class TimelineController : BaseApiController
    {
        [HttpGet]
        [ResponseType(typeof(GetTimelineDateResponse))]
        public async Task<HttpResponseMessage> GetTimelineContent(int childId)
        {
            GetTimelineContentPaginationRequest request = new GetTimelineContentPaginationRequest()
            {
                ChildId = childId
            };
            try
            {
                LoggerHelper.Info(string.Format("ChildId: {0}",childId));
                return GetFixedResponse(await new TimelineLogic().InitTimeline(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [ResponseType(typeof(GetTimelineDateResponse))]
        public async Task<HttpResponseMessage> GetPreviousWeekTimelineContent(int childId, int paTaskIdToSkip, int rtTaskIdToSkip, int minWeek, int maxWeek)
        {
            GetTimelineContentPaginationRequest request = new GetTimelineContentPaginationRequest()
            {
                ChildId = childId,
                PaTaskIdToSkip = paTaskIdToSkip,
                RtTaskIdToSkip = rtTaskIdToSkip,
                MinWeeks = minWeek,
                MaxWeeks = maxWeek
            };

            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new TimelineLogic().GetPreviousWeekTimelineContent(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [ResponseType(typeof(GetTimelineDateResponse))]
        public async Task<HttpResponseMessage> GetNextWeeksTimelineContent(int childId, int paTaskIdToSkip, int rtTaskIdToSkip, int minWeek, int maxWeek)
        {
            GetTimelineContentPaginationRequest request = new GetTimelineContentPaginationRequest()
            {
                ChildId = childId,
                PaTaskIdToSkip = paTaskIdToSkip,
                RtTaskIdToSkip = rtTaskIdToSkip,
                MinWeeks = minWeek,
                MaxWeeks = maxWeek
            };

            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new TimelineLogic().GetNextWeekTimelineContent(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [ResponseType(typeof(UpdateTaskResponse))]
        public HttpResponseMessage UpdateProActiveTask(UpdateProActiveTaskRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new TimelineLogic().UpdateProActiveTask(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }


        [HttpPost]
        [ResponseType(typeof(UpdateTaskResponse))]
        public HttpResponseMessage UpdateRealTimeTaskQuestion(UpdateRealTimeTaskQuestionRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new TimelineLogic().UpdateRealTimeQuestionTask(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }


        [HttpPost]
        [ResponseType(typeof(UpdateTaskResponse))]
        public HttpResponseMessage UpdateRealTimeTaskSolution(UpdateRealTimeTaskSolutionRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new TimelineLogic().UpdateRealTimeSolutionTask(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [ResponseType(typeof(UpdateTaskResponse))]
        public HttpResponseMessage UpdateRealTimeTaskSymptom(UpdateRealTimeTaskSymptomRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new TimelineLogic().UpdateRealTimeSymptomTask(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetTimelineItem(int childId, int taskId, TaskTypeEnum taskType)
        {
            try
            {

                return GetFixedResponse(await new TimelineLogic().GetTimelineItem(childId, taskId, taskType));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }
    }
}
