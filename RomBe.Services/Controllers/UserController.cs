using RomBe.Entities;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Response;
using RomBe.Entities.DAL;
using RomBe.Logic.Authentication.Logic;
using RomBe.Logic.UserLogic;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using RomBe.Services.Filters;
using RomBe.Entities.Class.Respone;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using RomBe.Entities.Class.Common;

namespace RomBe.Services.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(CreateUserTokenResponse))]
        [ValidateModel]
        public HttpResponseMessage CreateRombeUser([FromBody] CreateRombeUserRequest request)
        {
            try
            {

                LoggerHelper.Info(request);
                Boolean isValid;
                //Authorization: Basic YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg==
                //YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg== is client_id:client_secret encode in base64
                HttpResponseMessage validtionResponse = new AuthenticationLogic().ValidateClientSecretAndClientId(out isValid);
                if (!isValid)
                {
                    return validtionResponse;
                }
                else
                {
                    CreateUserTokenResponse result = new UserLogic().CreateRombeUser(request);
                    return GetFixedResponse(result);

                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(CreateUserTokenResponse))]
        [ValidateModel]
        public HttpResponseMessage FacebookLogin([FromBody] FacebookLoginRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                //Authorization: Basic YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg==
                //YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg== is client_id:client_secret encode in base64
                CreateUserTokenResponse result = new UserLogic().FacebookLogin(request);
                if (!result.IsNull())
                {
                    return GetFixedResponse(result);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }


            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(CreateUserTokenResponse))]
        [ValidateModel]
        public async Task<HttpResponseMessage> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                //Authorization: Basic YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg==
                //YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg== is client_id:client_secret encode in base64
                CreateUserTokenResponse result = await new UserLogic().GoogleLogin(request);
                if (!result.IsNull())
                {
                    return GetFixedResponse(result);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }


            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(LoginResponse))]
        [ValidateModel]
        public HttpResponseMessage Login([FromBody] CreateUserObject request)
        {
            try
            {
                LoggerHelper.Info(request);
                //Authorization: Basic YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg==
                //YW5kcm9pZEFwcDo1OUZERUVFNjQ4NkNBNzk2QTEzQUU1NDM3RDJCMg== is client_id:client_secret encode in base64
                LoginResponse result = new UserLogic().Login(request);
                if (!result.IsNull())
                {
                    return GetFixedResponse(result);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }


            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }


        [HttpPost]
        [ResponseType(typeof(RecoverPasswordResponse))]
        public HttpResponseMessage RcoverPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new UserLogic().RecoverPassword(request));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }

        }

        [HttpPost]
        [ResponseType(typeof(RecoverEmailResponse))]
        public HttpResponseMessage RecoverEmail([FromBody] RecoverEmailRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new UserLogic().RecoverEmail(request));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }

        }

        [HttpPost]
        [Authorize]
        [ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new UserLogic().UpdateUser(request));
            }
            catch (Exception e)
            {

                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [Authorize]
        [ResponseType(typeof(GetUserDetailsResponse))]
        public HttpResponseMessage GetUserDetails()
        {
            try
            {
                return GetFixedResponse(new UserLogic().GetUserDetails());
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, new AuthenticationHelper().GetCurrentUserId());
                return GlobalExeption();
            }
        }

        [HttpGet]
        [ResponseType(typeof(LogOutResponse))]
        public async Task<HttpResponseMessage> LogOut(int userId)
        {
            try
            {
                LoggerHelper.Info(string.Format("UserId: {0}",userId));
                return GetFixedResponse(await new UserLogic().LogOut(userId));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [Authorize]
        [ResponseType(typeof(GetFieldsToShowResponse))]
        public HttpResponseMessage GetFieldsToShow()
        {
            try
            {
                return GetFixedResponse(new UserLogic().GetFieldsToShow());
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, new AuthenticationHelper().GetCurrentUserId().Value);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [Authorize]
        [ResponseType(typeof(BaseResponse))]
        public HttpResponseMessage ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(new UserLogic().ChangePassword(request).HttpStatusCode);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

    }
}
