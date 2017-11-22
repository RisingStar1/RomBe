using RomBe.Entities.Class;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Response;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;

namespace RomBe.Entities.DAL
{
    public class UserDAL
    {

        #region public methods

        /// <summary>
        /// return null if the id of the user is not exist
        /// return the user object if exist
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns></returns>
        public User GetUser(int userId, bool extandSelect = false)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    if (!extandSelect)
                    {
                        return context.Users.SingleOrDefault(u => u.UserId == userId);
                    }
                    else
                    {
                        return context.Users.
                               Include(a => a.Children)
                               .Include(a => a.PregnantDetails)
                               .SingleOrDefault(u => u.UserId == userId);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<User> GetUser(String userName, String password)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User user = context.Users.Where(u => u.Email == userName && u.Password == password).FirstOrDefault();
                    if (!user.IsNull())
                        return await context.Users.FindAsync(user.UserId);

                    else return null;

                }
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
                using (RombeEntities context = new RombeEntities())
                {
                    User user = context.Users.Where(u => u.Email == email).FirstOrDefault();
                    if (!user.IsNull())
                        return user;

                    else return null;

                }
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
                using (RombeEntities context = new RombeEntities())
                {
                    User user = (from u in context.Users
                                 join fb in context.FacebookProviders on u.UserId equals fb.UserId
                                 where fb.FacebookUserId == facebookUserId &&
                                       u.IsActive
                                 select u).FirstOrDefault();

                    if (!user.IsNull())
                        return await context.Users.FindAsync(user.UserId);

                    else return null;

                }
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
                using (RombeEntities context = new RombeEntities())
                {
                    User user = (from u in context.Users
                                 join google in context.GoogleProviders on u.UserId equals google.UserId
                                 where google.GoogleUserId == googleUserId &&
                                       u.IsActive
                                 select u).FirstOrDefault();

                    if (!user.IsNull())
                        return await context.Users.FindAsync(user.UserId);

                    else return null;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserByEmailAddressAsync(string email)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    return await context.Users.SingleOrDefaultAsync(u => u.Email == email);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CreateRombeUser(CreateRombeUserRequest request)
        {
            int result = 0;
            bool isSucceed = false;
            try
            {
                User newUser = new User();
                using (RombeEntities context = new RombeEntities())
                {

                    CommonCreateUser(request, newUser);

                    newUser.Password = request.Password;                 
                    newUser.RegistartionSource = (byte)RegistartionSourceTypeEnum.Rombe;
                    newUser.IsActive = false;
                    newUser.CampaignSource = (byte)request.CampaignSource;
                    
                    context.UserEmailActivations.Add(CreateAccountActivation(newUser.UserId));
                    context.Users.Add(newUser);


                    result = context.SaveChanges();
                    isSucceed = Convert.ToBoolean(result);
                }
                if (isSucceed)
                {
                    if (!request.UserDeviceObject.IsNull())
                    {
                        Task.Run(() => new UserDeviceDAL().CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId));
                        //await CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId).ConfigureAwait(false);
                    }
                }
                return isSucceed;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public int CreateUser(CreateUserObject request)
        {
            int result = 0;
            bool isSucceed = false;
            try
            {
                User newUser = new User();
                using (RombeEntities context = new RombeEntities())
                {
                    request.FirstName = string.Empty;
                    request.LastName = string.Empty;
                    CommonCreateUser(request, newUser);
                    newUser.RegistartionSource = (byte)RegistartionSourceTypeEnum.Rombe;
                    newUser.IsActive = true;
                    newUser.CampaignSource = (byte)request.CampaignSource;
                    context.Users.Add(newUser);


                    result = context.SaveChanges();
                    isSucceed = Convert.ToBoolean(result);
                }
                if (isSucceed)
                {
                    if (!request.UserDeviceObject.IsNull())
                    {
                        Task.Run(() => new UserDeviceDAL().CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId));
                        //await CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId).ConfigureAwait(false);
                    }
                }
                return newUser.UserId;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public bool CreateFacebookUser(CreateFacebookUserRequest request)
        {
            int result = 0;
            try
            {
                User newUser = new User();
                using (RombeEntities context = new RombeEntities())
                {
                    CommonCreateUser(request, newUser);

                    newUser.IsActive = true;
                    newUser.GenderId = (int)request.GenderType;
                    newUser.RegistartionSource = (byte)RegistartionSourceTypeEnum.Facebook;

                    //Only if we get the birthdate we calculate the age
                    if (request.BirthDate.HasValue)
                    {
                        newUser.BirthDate = request.BirthDate.Value;
                        newUser.Age = CalculationHelper.CalculateAge(request.BirthDate.Value);
                    }

                    context.FacebookProviders.Add(CreateFacebookProvider(newUser.UserId, request.FacebookUserId));
                    context.Users.Add(newUser);


                    result = context.SaveChanges();

                }
                if (Convert.ToBoolean(result))
                {
                    if (!request.UserDeviceObject.IsNull())
                    {
                        Task.Run(() => new UserDeviceDAL().CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId));
                        //await CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId).ConfigureAwait(false);
                    }
                }
                return Convert.ToBoolean(result);
            }

            catch (Exception)
            {
                throw;
            }



        }

        public bool CreateGoogleUser(CreateGoogleUserRequest request)
        {
            int result = 0;
            try
            {
                User newUser = new User();
                using (RombeEntities context = new RombeEntities())
                {
                    CommonCreateUser(request, newUser);

                    newUser.IsActive = true;
                    newUser.GenderId = (int)request.GenderType;
                    newUser.RegistartionSource = (byte)RegistartionSourceTypeEnum.Google;
                    

                    context.GoogleProviders.Add(CreateGoogleProvider(newUser.UserId, request.GoogleUserId));
                    context.Users.Add(newUser);


                    result = context.SaveChanges();

                }
                if (Convert.ToBoolean(result))
                {
                    if (!request.UserDeviceObject.IsNull())
                    {
                        Task.Run(() => new UserDeviceDAL().CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId));
                        //await CreateUserDeviceAsync(request.UserDeviceObject, newUser.UserId).ConfigureAwait(false);
                    }
                }
                return Convert.ToBoolean(result);
            }

            catch (Exception)
            {
                throw;
            }



        }

        public IsUserExistObject IsUserExist(String userName)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User isUserExist = context.Users.Where(u => u.Email == userName)
                                      .Include(u => u.FacebookProviders)
                                      .Include(u => u.GoogleProviders)
                                      .FirstOrDefault();
                    if (!isUserExist.IsNull())
                    {
                        return new IsUserExistObject
                        {
                            IsExist = true,
                            UserRegistartionSourceType = (RegistartionSourceTypeEnum)isUserExist.RegistartionSource,
                            FacebookUserId = isUserExist.FacebookProviders.Any() ? isUserExist.FacebookProviders.FirstOrDefault().FacebookUserId : string.Empty,
                            GoogleUserId = isUserExist.GoogleProviders.Any() ? isUserExist.GoogleProviders.FirstOrDefault().GoogleUserId : string.Empty,
                            IsActiveUser = isUserExist.IsActive,
                            Password = isUserExist.Password,
                            UserId=isUserExist.UserId
                        };
                    }
                    else
                    {
                        return new IsUserExistObject();
                    }
                }
            }
            catch (DbEntityValidationException validationExeption)
            {
                foreach (var validationErrors in validationExeption.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //TOOD FIX THE RECOVER EMAIL
        public String RecoverEmail(RecoverEmailRequest request)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User result = new User();//context.Users.Where(u => u.PhoneNumber == request.PhoneNumber).FirstOrDefault();
                    if (!result.IsNull())
                        return result.Email;
                    else return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Boolean> UpdateUser(UpdateUserRequest request, int userId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User loggedInUser = context.Users.Where(u => u.UserId == userId).Include(c => c.Children).FirstOrDefault();

                    if (loggedInUser.IsNull())
                    {
                        return false;
                    }
                    else
                    {
                        UpdateUserDetails(request, loggedInUser);

                        if (!request.PregnantObject.IsNull())
                        {
                            if (request.PregnantObject.IsPregnant &&
                                !request.PregnantObject.LastPeriodDate.IsNull() &&
                                request.PregnantObject.LastPeriodDate != DateTime.MinValue)
                            {
                                PregnantDetail newPregnantDetail = CreatePregnantDetails(request, loggedInUser);
                                context.PregnantDetails.Add(newPregnantDetail);
                                loggedInUser.IsPregnant = true;
                            }
                            else
                            {
                                PregnantDetail currentPregnant = context.PregnantDetails.Where(p => p.UserId == userId).FirstOrDefault();
                                if (!currentPregnant.IsNull())
                                {
                                    context.PregnantDetails.Remove(currentPregnant);
                                }

                                loggedInUser.IsPregnant = false;
                            }
                        }

                        loggedInUser.UpdateDate = DateTime.Now;
                        int updateResult = await context.SaveChangesAsync().ConfigureAwait(false);
                        return Convert.ToBoolean(updateResult);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void UpdateUserLoginDetails(UpdateUserPartialObject updateUserObject)
        {
            using (RombeEntities context = new RombeEntities())
            {
                User userToUpdate = context.Users.Where(u => u.Email == updateUserObject.Email).FirstOrDefault();
                if (!userToUpdate.IsNull() && !updateUserObject.IsNull())
                {
                    if (!updateUserObject.Password.IsNull())
                    {
                        userToUpdate.Password = updateUserObject.Password;
                    }
                    if (updateUserObject.IsActive.HasValue)
                    {
                        userToUpdate.IsActive = updateUserObject.IsActive.Value;
                    }
                    if (!updateUserObject.FacebookUserId.IsNull() && !userToUpdate.FacebookProviders.Any())
                    {
                        context.FacebookProviders.Add(CreateFacebookProvider(userToUpdate.UserId, updateUserObject.FacebookUserId));   
                    }

                    if (!updateUserObject.GoogleUserId.IsNull() && !userToUpdate.GoogleProviders.Any())
                    {
                        context.GoogleProviders.Add(CreateGoogleProvider(userToUpdate.UserId, updateUserObject.GoogleUserId));
                    }

                    context.SaveChanges();
                }
            }
        }

        public bool? ActiveUserAccount(string activationKey)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    UserEmailActivation temp = context.UserEmailActivations.Where(u => u.UserAccountActivationKey == activationKey).FirstOrDefault();
                    if (temp.IsNull())
                    {
                        return null;
                    }
                    User currentUser = context.Users.Where(u => u.UserId == temp.UserId).FirstOrDefault();
                    if (!currentUser.IsNull())
                    {
                        currentUser.IsActive = true;
                        context.UserEmailActivations.Remove(temp);
                        int result = context.SaveChanges();
                        return Convert.ToBoolean(result);
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return null;
            }

        }
        public PregnantObject GetPragnetDetails(int userId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                PregnantDetail result = context.PregnantDetails.Where(p => p.UserId == userId).OrderByDescending(p => p.InsertDate).FirstOrDefault();
                if (!result.IsNull())
                {
                    return new PregnantObject
                    {
                        IsPregnant = true,
                        LastPeriodDate = result.DateOfLastMenstrual
                    };
                }
                return null;
            }
        }
        public bool ChangePassword(ChangePasswordRequest request, int userId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                User currentUser = context.Users.Where(u => u.UserId == userId).FirstOrDefault();
                if (!currentUser.IsNull())
                {
                    currentUser.Password = request.NewPassword;
                    return Convert.ToBoolean(context.SaveChanges());
                }
                return false;
            }
        }

        public bool IsTheChildRelatedToTheUser(int userId, int childId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return (from user in context.Users
                        join child in context.Children on user.UserId equals child.UserId
                        where user.UserId == userId && child.ChildId == childId
                        select child).Any();
            }
        }
        #endregion public methods

        #region private methods

        private UserEmailActivation CreateAccountActivation(int userId)
        {
            UserEmailActivation newUserAccountActivation = new UserEmailActivation();
            newUserAccountActivation.UserId = userId;
            newUserAccountActivation.UserAccountActivationKey = GenerateActivationKey();
            newUserAccountActivation.InsertDate = DateTime.Now;

            return newUserAccountActivation;
        }

        private FacebookProvider CreateFacebookProvider(int userId, string facebookUserId)
        {
            FacebookProvider newFacebookProvider = new FacebookProvider();
            newFacebookProvider.UserId = userId;
            newFacebookProvider.FacebookUserId = facebookUserId;
            newFacebookProvider.InsertDate = DateTime.Now;

            return newFacebookProvider;
        }

        private GoogleProvider CreateGoogleProvider(int userId, string googleUserId)
        {
            GoogleProvider newGoogleProvider = new GoogleProvider();
            newGoogleProvider.UserId = userId;
            newGoogleProvider.GoogleUserId = googleUserId;
            newGoogleProvider.InsertDate = DateTime.Now;

            return newGoogleProvider;
        }

        private string GenerateActivationKey()
        {
            return string.Format("{0}{1}{2}", string.Format("{0:x}", Guid.NewGuid().ToString("n")),
                                              string.Format("{0:x}", Guid.NewGuid().ToString("n")),
                                              DateTime.Now.Ticks);
        }

        private void UpdateUserDetails(UpdateUserRequest request, User loggedInUser)
        {
            if (!request.BirthDate.IsNull())
            {
                if (loggedInUser.BirthDate.IsNull())
                {
                    loggedInUser.BirthDate = request.BirthDate;
                    loggedInUser.Age = CalculationHelper.CalculateAge(request.BirthDate.Value);
                }
                else if (loggedInUser.BirthDate.Value != request.BirthDate.Value)
                {
                    loggedInUser.BirthDate = request.BirthDate;
                    loggedInUser.Age = CalculationHelper.CalculateAge(request.BirthDate.Value);
                }
            }
            //update gender
            if (!request.Gender.IsNull())
            {
                if (loggedInUser.GenderId.IsNull())
                {
                    loggedInUser.GenderId = (int?)request.Gender;
                }
                else if (loggedInUser.GenderId != (int?)request.Gender)
                {
                    loggedInUser.GenderId = (int?)request.Gender;
                }
            }

            if (!request.Language.IsNull())
            {
                if (loggedInUser.LanguageId.IsNull())
                {
                    loggedInUser.LanguageId = (int)request.Language;
                }
                else if (loggedInUser.LanguageId != (int?)request.Language)
                {
                    loggedInUser.LanguageId = (int)request.Language;
                }
            }

            if (!request.FirstName.IsNull())
            {
                if (loggedInUser.FirstName.IsNull())
                {
                    loggedInUser.FirstName = request.FirstName;
                }
                else if (loggedInUser.FirstName != request.FirstName)
                {
                    loggedInUser.FirstName = request.FirstName;
                }
            }
            if (!request.LastName.IsNull())
            {
                if (loggedInUser.LastName.IsNull())
                {
                    loggedInUser.LastName = request.LastName;
                }
                else if (loggedInUser.LastName != request.LastName)
                {
                    loggedInUser.LastName = request.LastName;
                }
            }
        }

        private PregnantDetail CreatePregnantDetails(UpdateUserRequest request, User loggedInUser)
        {

            return new PregnantDetail()
            {
                DateOfLastMenstrual = request.PregnantObject.LastPeriodDate,
                UserId = loggedInUser.UserId,
                UpdateDate = DateTime.Now,
                InsertDate = DateTime.Now
            };

        }

        private void CommonCreateUser(CreateUserObject request, User newUser)
        {
            newUser.Email = request.Email;
            newUser.City = request.City;
            newUser.CountryId = request.CountryId;
            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.LanguageId = (int)LanguagesEnum.English;
            newUser.LocalTime = request.LocalTime;
            newUser.CampaignSource = (byte)request.CampaignSource;
            newUser.IpAddress = request.IpAddress;
            newUser.InsertDate = DateTime.Now;
            newUser.UpdateDate = DateTime.Now;
        }

        #endregion private methods

    }
}
