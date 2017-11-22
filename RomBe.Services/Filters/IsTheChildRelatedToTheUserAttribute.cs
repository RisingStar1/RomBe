using RomBe.Entities.DAL;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using RomBe.Entities.Class.Timeline;
using System.Collections.Specialized;

namespace RomBe.Services.Filters
{
    public class IsTheChildRelatedToTheUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            int _childIdAsint = 0;
            if (actionContext.Request.Method == HttpMethod.Get)
            {

                if (!actionContext.Request.RequestUri.Query.IsNull())
                {
                    NameValueCollection queryStringList = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
                    string _childIdAsString = queryStringList.Get("childId");
                    if (!int.TryParse(_childIdAsString, out _childIdAsint))
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    }
                }
            }
            else
            {
                if (actionContext.ActionArguments.ContainsKey("request"))
                {
                    BaseUpdateTaskRequest request = (BaseUpdateTaskRequest)actionContext.ActionArguments["request"];
                    _childIdAsint = request.ChildId;
                }
            }


            int? _userId = new AuthenticationHelper().GetCurrentUserId();
            bool _isTheChildRelatedToTheUser = new UserDAL().IsTheChildRelatedToTheUser(_userId.Value, _childIdAsint);
            if (!_isTheChildRelatedToTheUser)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            }

        }
    }
}