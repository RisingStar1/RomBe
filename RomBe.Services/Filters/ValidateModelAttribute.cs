using RomBe.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace RomBe.Services.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                LoggerHelper.Info(actionContext.ActionArguments);
                if (SystemConfigurationHelper.IsProduction)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Arguments cannot be null");
                }

            }

            if (!actionContext.ModelState.IsValid)
            {
                LoggerHelper.Info(actionContext.ActionArguments);
                if (SystemConfigurationHelper.IsProduction)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, actionContext.ModelState.Keys);
                }
            }
        }

    }
}