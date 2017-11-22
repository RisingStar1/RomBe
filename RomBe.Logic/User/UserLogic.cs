using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Entities;
using RomBe.Entities.DAL;
using RomBe.Entities.Class.Request;
using RomBe.Helpers;
using RomBe.Logic.Authentication.Logic;
using RomBe.Logic.Facebook;
using RomBe.Entities.Class.Response;
using RomBe.Logic.Notification;
using RomBe.Entities.Class.Respone;
using System.Net;
using System.Web.Hosting;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web;
using RomBe.Logic.Google;

namespace RomBe.Logic.UserLogic
{
    public class UserLogic
    {
        const int UNKNOW_COUNTRY_CODE = 141;

        #region public methods
        //use in the token service
        public async Task<User> GetUser(String userName, String password)
        {
            try
            {
                return await new UserDAL().GetUser(userName, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserByEmail(String email)
        {
            try
            {
                return new UserDAL().GetUserByEmail(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserByFacebookId(String facebookUserId)
        {
            try
            {
                return await new UserDAL().GetUserByFacebookId(facebookUserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserByGoogleId(String googleUserId)
        {
            try
            {
                return await new UserDAL().GetUserByGoogleId(googleUserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CreateUserTokenResponse CreateRombeUser(CreateRombeUserRequest request)
        {
            try
            {
                UserDAL userDal = new UserDAL();
                LocationHelper _locationHelper = new LocationHelper();
                bool isSucceedToRegister = false;
                IsUserExistObject isUserExist = userDal.IsUserExist(request.Email);
                if (!isUserExist.IsExist)
                {
                    SetUserLocationDetails(request, _locationHelper);

                    isSucceedToRegister = userDal.CreateRombeUser(request);
                    return RombeUserCreated(request.Email, isSucceedToRegister);
                }
                else if (isUserExist.UserRegistartionSourceType == RegistartionSourceTypeEnum.Facebook)
                {
                    if (isUserExist.Password.IsNull())
                    {
                        UpdateUserPartialObject updateUser = new UpdateUserPartialObject()
                        {
                            Email = request.Email,
                            Password = request.Password
                        };
                        userDal.UpdateUserLoginDetails(updateUser);
                        //go to the token api to get token
                        CreateUserTokenResponse result = new AuthenticationLogic().GetTokenForFacebookUser(isUserExist.FacebookUserId);
                        result.HttpStatusCode = HttpStatusCode.OK;
                        return result;
                    }
                    else
                    {
                        return new CreateUserTokenResponse()
                        {
                            HttpStatusCode = HttpStatusCode.Conflict,
                            MessageCode = SystemConfigurationHelper.EmailIsExistMessageCode
                        };
                    }

                }
                else if (isUserExist.IsActiveUser)
                {
                    LoggerHelper.Info(request);
                    return new CreateUserTokenResponse()
                    {
                        HttpStatusCode = HttpStatusCode.Conflict,
                        MessageCode = SystemConfigurationHelper.EmailIsExistMessageCode
                    };
                }
                else
                {
                    LoggerHelper.Info(request);
                    return new CreateUserTokenResponse()
                    {
                        HttpStatusCode = HttpStatusCode.Unauthorized,
                        MessageCode = SystemConfigurationHelper.EmailNotVerifidMessageCode
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetUserLocationDetails(CreateUserObject request, LocationHelper _locationHelper)
        {
            request.IpAddress = _locationHelper.GetUserIp();
            LocationResponse userLocation = _locationHelper.GetDetailsByIP();
            if (!userLocation.IsNull())
            {
                request.CountryId = new CountryDAL().GetCountryIdByName(userLocation.Country);
                request.City = userLocation.City;
            }
            else
            {
                request.CountryId = UNKNOW_COUNTRY_CODE;
            }
        }

        public CreateUserTokenResponse FacebookLogin(FacebookLoginRequest request)
        {
            try
            {
                LocationHelper _locationHelper = new LocationHelper();
                UserDAL userDal = new UserDAL();
                bool isSucceedToRegister = false;
                if (request.IsNull() || request.FacebookToken.IsNull())
                {
                    LoggerHelper.Info(request);
                    //return error code no token
                    return new CreateUserTokenResponse()
                    {
                        HttpStatusCode = HttpStatusCode.Forbidden
                    };
                }
                //handle exeptions
                CreateFacebookUserRequest userDetailsFromFacebook = new FacebookLogic().GetUserDetails(request);
                if (userDetailsFromFacebook.IsNull())
                {
                    return null;
                }

                IsUserExistObject isUserExist = userDal.IsUserExist(userDetailsFromFacebook.Email);

                if (isUserExist.IsExist)
                {
                    return FacebookUserIsExist(userDetailsFromFacebook, isUserExist);
                }


                SetUserLocationDetails(userDetailsFromFacebook, _locationHelper);

                isSucceedToRegister = userDal.CreateFacebookUser(userDetailsFromFacebook);
                if (isSucceedToRegister)
                {
                    CreateUserTokenResponse result = new AuthenticationLogic().GetTokenForFacebookUser(userDetailsFromFacebook.FacebookUserId);
                    result.HttpStatusCode = HttpStatusCode.Created;
                    return result;
                }
                else
                {
                    LoggerHelper.Info(request);
                    return new CreateUserTokenResponse()
                    {
                        MessageCode = SystemConfigurationHelper.InternalErrorMessageCode,
                        HttpStatusCode = HttpStatusCode.InternalServerError
                    };
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CreateUserTokenResponse> GoogleLogin(GoogleLoginRequest request)
        {
            try
            {
                LocationHelper _locationHelper = new LocationHelper();
                UserDAL userDal = new UserDAL();
                bool isSucceedToRegister = false;
                if (request.IsNull() || request.GoogleToken.IsNull())
                {
                    LoggerHelper.Info(request);
                    //return error code no token
                    return new CreateUserTokenResponse()
                    {
                        HttpStatusCode = HttpStatusCode.Forbidden
                    };
                }
                //handle exeptions
                CreateGoogleUserRequest userDetailsFromGoogle = await new GoogleLogic().GetUserDetails(request);
                if (userDetailsFromGoogle.IsNull())
                {
                    return null;
                }

                IsUserExistObject isUserExist = userDal.IsUserExist(userDetailsFromGoogle.Email);

                if (isUserExist.IsExist)
                {
                    return GoogleUserIsExist(userDetailsFromGoogle, isUserExist);
                }


                SetUserLocationDetails(userDetailsFromGoogle, _locationHelper);

                isSucceedToRegister = userDal.CreateGoogleUser(userDetailsFromGoogle);
                if (isSucceedToRegister)
                {
                    CreateUserTokenResponse result = new AuthenticationLogic().GetTokenForFacebookUser(userDetailsFromGoogle.GoogleUserId);
                    result.HttpStatusCode = HttpStatusCode.Created;
                    return result;
                }
                else
                {
                    LoggerHelper.Info(request);
                    return new CreateUserTokenResponse()
                    {
                        MessageCode = SystemConfigurationHelper.InternalErrorMessageCode,
                        HttpStatusCode = HttpStatusCode.InternalServerError
                    };
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public LoginResponse Login(CreateUserObject request)
        {
            try
            {
                LocationHelper _locationHelper = new LocationHelper();
                UserDAL userDal = new UserDAL();
                if (request.IsNull() || request.Email.IsNull())
                {
                    LoggerHelper.Info(request);
                    //return error code no token
                    return new LoginResponse()
                    {
                        HttpStatusCode = HttpStatusCode.Forbidden
                    };
                }

                IsUserExistObject isUserExist = userDal.IsUserExist(request.Email);

                if (isUserExist.IsExist)
                {
                    return UserIsExist(request.Email, isUserExist.UserId);
                }
                
                SetUserLocationDetails(request, _locationHelper);

                int userId = userDal.CreateUser(request);
                if (userId != 0)
                {
                    return UserIsExist(request.Email, userId);
                }

                return new LoginResponse()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    MessageCode = SystemConfigurationHelper.InternalErrorMessageCode,
                };


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RecoverPasswordResponse> RecoverPassword(ResetPasswordRequest request)
        {
            try
            {

                if (!request.Email.IsNull())
                {
                    await new EmailLogic().SendRecoverPasswordEmailAsync(request.Email);
                }

                return new RecoverPasswordResponse()
                {
                    SucceedToRecoverPassword = true
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RecoverEmailResponse RecoverEmail(RecoverEmailRequest request)
        {
            try
            {
                RecoverEmailResponse response = new RecoverEmailResponse();
                if (!String.IsNullOrEmpty(request.PhoneNumber))
                {
                    String email = new UserDAL().RecoverEmail(request);
                    if (!email.IsNull())
                    {
                        response.Founded = true;
                        response.Email = email;
                    }
                }
                else
                {
                    response.Founded = false;
                    response.Email = null;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetUserDetailsResponse> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                GetUserDetailsResponse response = new GetUserDetailsResponse();
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                if (!request.IsNull())
                {
                    int? userId = new AuthenticationHelper().GetCurrentUserId();
                    if (!userId.HasValue)
                    {
                        return response;
                    }
                    User _currentUser = new UserDAL().GetUser(userId.Value);
                    if (_currentUser.IsNull())
                    {
                        return response;
                    }

                    Boolean updateSucceeded = await new UserDAL().UpdateUser(request, userId.Value);
                    if (updateSucceeded)
                    {
                        response = GetUserDetails();
                        response.HttpStatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        LoggerHelper.Info(request);
                    }

                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GetUserDetailsResponse GetUserDetails(int userId = 0)
        {
            try
            {
                int? currentLoggedInUser = new AuthenticationHelper().GetCurrentUserId();
                GetUserDetailsResponse response = new GetUserDetailsResponse();


                if (!currentLoggedInUser.HasValue && userId != 0)
                {
                    currentLoggedInUser = userId;
                }


                if (!currentLoggedInUser.HasValue)
                {
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                User _currentUser = new UserDAL().GetUser(currentLoggedInUser.Value);
                if (_currentUser.IsNull())
                {
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    return response;
                }


                response = GetDetailsOfUser(currentLoggedInUser.Value);
                response.ChildsList = new ChildLogic.ChildLogic().GetChildsList(currentLoggedInUser.Value);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> LogOut(int userId)
        {
            return await new NotificationLogic().UnSubscribeNotification(userId).ConfigureAwait(false);
        }

        public HttpStatusCode AccountActivation(string activationCode)
        {
            try
            {
                bool? result = new UserDAL().ActiveUserAccount(activationCode);
                if (result.HasValue && result.Value)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    LoggerHelper.Info(activationCode);
                    return HttpStatusCode.NotFound;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GetFieldsToShowResponse GetFieldsToShow()
        {

            const string SETTINGS_PASSWORD_FIELD_NAME = "Password";

            GetFieldsToShowResponse response = new GetFieldsToShowResponse();

            int? _userId = new AuthenticationHelper().GetCurrentUserId();

            if (!_userId.HasValue)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            User _loggedInUser = new UserDAL().GetUser(_userId.Value, true);
            if (_loggedInUser.IsNull())
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                return response;
            }


            if (_loggedInUser.BirthDate.IsNull() || _loggedInUser.GenderId.IsNull() ||
                (!_loggedInUser.Children.Any() && !_loggedInUser.PregnantDetails.Any()) || _loggedInUser.LanguageId == 0)
            {
                response.MissingDetailsScreen = new Dictionary<string, bool>();
                GetMissingDetailsScreenFieldsToHide(_loggedInUser, response);

            }

            if (_loggedInUser.Password.IsNull())
            {
                response.SettingsScreen = new Dictionary<string, bool>();
                response.SettingsScreen.Add(SETTINGS_PASSWORD_FIELD_NAME, true);
            }

            return response;
        }

        public BaseResponse ChangePassword(ChangePasswordRequest request)
        {
            BaseResponse response = new BaseResponse();
            response.HttpStatusCode = HttpStatusCode.BadRequest;

            bool updateResult, isNewPasswordValid;
            isNewPasswordValid = new ValidationHelper().IsValidPassword(request.NewPassword);
            if (isNewPasswordValid)
            {
                int? _currentUserId = new AuthenticationHelper().GetCurrentUserId();
                if (!_currentUserId.HasValue)
                {
                    return response;
                }
                User currentUser = new UserDAL().GetUser(_currentUserId.Value);
                if (currentUser.IsNull())
                {
                    return response;
                }
                if (currentUser.Password != request.OldPassword)
                {
                    LoggerHelper.Info(request);
                    response.MessageCode = SystemConfigurationHelper.CurrentPasswordNotMatchCode;
                }
                else
                {
                    updateResult = new UserDAL().ChangePassword(request, _currentUserId.Value);
                    if (updateResult)
                    {
                        response.HttpStatusCode = HttpStatusCode.OK;
                    }
                }

            }

            return response;
        }


        #endregion public methods

        #region private methods

        private CreateUserTokenResponse RombeUserCreated(string email, bool isSucceedToRegister)
        {
            if (isSucceedToRegister)
            {
                HostingEnvironment.QueueBackgroundWorkItem(task =>
                {
#pragma warning disable 4014
                    new EmailLogic().SendActivationEmailAsync(email);
                });

                return new CreateUserTokenResponse()
                {
                    HttpStatusCode = HttpStatusCode.Created,
                    MessageCode = SystemConfigurationHelper.SucceedToRegisterViaRombeMessageCode
                };
            }
            else
            {
                LoggerHelper.Info(email);
                return new CreateUserTokenResponse()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    MessageCode = SystemConfigurationHelper.InternalErrorMessageCode
                };
            }
        }

        private CreateUserTokenResponse FacebookUserIsExist(CreateFacebookUserRequest request, IsUserExistObject isUserExist)
        {
            CreateUserTokenResponse result = null;
            if (isUserExist.UserRegistartionSourceType == RegistartionSourceTypeEnum.Facebook)
            {
                //CreateUserDeviceAsync

                //go to the token api to get token
                result = new AuthenticationLogic().GetTokenForFacebookUser(isUserExist.FacebookUserId);
                result.HttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                //the user is register as rombe user but not verify is email 

                UpdateUserPartialObject updateUserObject = new UpdateUserPartialObject()
                {
                    Email = request.Email,
                    FacebookUserId = request.FacebookUserId,
                    IsActive = true
                };
                new UserDAL().UpdateUserLoginDetails(updateUserObject);
                result = new AuthenticationLogic().GetTokenForFacebookUser(request.FacebookUserId);
                result.HttpStatusCode = HttpStatusCode.OK;


            }
            return result;
        }

        private CreateUserTokenResponse GoogleUserIsExist(CreateGoogleUserRequest request, IsUserExistObject isUserExist)
        {
            CreateUserTokenResponse result = null;
            if (isUserExist.UserRegistartionSourceType == RegistartionSourceTypeEnum.Google)
            {
                //CreateUserDeviceAsync

                //go to the token api to get token
                result = new AuthenticationLogic().GetTokenForFacebookUser(isUserExist.GoogleUserId);
                result.HttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                //the user is register as rombe user but not verify is email 

                UpdateUserPartialObject updateUserObject = new UpdateUserPartialObject()
                {
                    Email = request.Email,
                    GoogleUserId = request.GoogleUserId,
                    IsActive = true
                };
                new UserDAL().UpdateUserLoginDetails(updateUserObject);
                result = new AuthenticationLogic().GetTokenForFacebookUser(request.GoogleUserId);
                result.HttpStatusCode = HttpStatusCode.OK;


            }
            return result;
        }

        private LoginResponse UserIsExist(string email, int userId)
        {

            LoginResponse response = new LoginResponse();
            //CreateUserDeviceAsync

            //go to the token api to get token
            response.UserToken = new AuthenticationLogic().GetTokenForUser(email);
            response.UserDetails = GetUserDetails(userId);
            response.HttpStatusCode = HttpStatusCode.OK;

            return response;
        }

        private GetUserDetailsResponse GetDetailsOfUser(int userId)
        {
            User currentUser = new UserDAL().GetUser(userId);
            GetUserDetailsResponse returnValue = new GetUserDetailsResponse();
            if (!currentUser.IsNull())
            {
                returnValue.UserId = currentUser.UserId;
                returnValue.FirstName = currentUser.FirstName;
                returnValue.LastName = currentUser.LastName;
                returnValue.BirthDate = currentUser.BirthDate;
                returnValue.Preferences.NotificationsEnabled = currentUser.SendNotifications;
                if (!currentUser.GenderId.IsNull())
                {
                    returnValue.Gender = (GenderTypeEnum)currentUser.GenderId;
                }
                else
                {
                    returnValue.Gender = null;
                }
                returnValue.Language = (LanguagesEnum)currentUser.LanguageId;
                returnValue.Country = currentUser.CountryId;
                returnValue.Email = currentUser.Email;
                PregnantObject pregnantDetails = new UserDAL().GetPragnetDetails(userId);
                if (pregnantDetails.IsNull())
                {
                    returnValue.LastPeriodDate = null;
                }
                else
                {
                    returnValue.LastPeriodDate = pregnantDetails.LastPeriodDate;
                }
            }
            return returnValue;
        }

        private void GetMissingDetailsScreenFieldsToHide(User loggedInUser, GetFieldsToShowResponse response)
        {
            const string MISSING_DETAILS_BIRTH_DATE_FIELD_NAME = "BirthDate";
            const string MISSING_DETAILS_GENDER_FIELD_NAME = "Gender";
            const string MISSING_DETAILS_OFFSPRING_FIELD_NAME = "Offspring";
            const string MISSING_DETAILS_PREGNANT_FIELD_NAME = "Pregnant";
            const string MISSING_DETAILS_LANGUAGE_FIELD_NAME = "Language";

            if (loggedInUser.BirthDate.IsNull())
            {
                response.MissingDetailsScreen[MISSING_DETAILS_BIRTH_DATE_FIELD_NAME] = true;
            }
            if (loggedInUser.GenderId.IsNull())
            {
                response.MissingDetailsScreen[MISSING_DETAILS_GENDER_FIELD_NAME] = true;
            }
            if (!loggedInUser.Children.Any() && !loggedInUser.PregnantDetails.Any())
            {
                response.MissingDetailsScreen[MISSING_DETAILS_OFFSPRING_FIELD_NAME] = true;
                response.MissingDetailsScreen[MISSING_DETAILS_PREGNANT_FIELD_NAME] = true;
            }
            if (loggedInUser.LanguageId == 0)
            {
                response.MissingDetailsScreen[MISSING_DETAILS_LANGUAGE_FIELD_NAME] = true;
            }
        }



        #endregion private methods

    }
}
