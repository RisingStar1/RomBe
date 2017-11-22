using Facebook;
using RomBe.Entities.Class.Request;
using RomBe.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Entities.Class;
using RomBe.Entities.Enums;
using RomBe.Helpers;

namespace RomBe.Logic.Facebook
{
    public class FacebookLogic
    {
        #region public methods
        public CreateFacebookUserRequest GetUserDetails(FacebookLoginRequest request)
        {
            try
            {
                FacebookClient facebookClient = new FacebookClient(request.FacebookToken);
                
                dynamic me = facebookClient.Get("/me");
               
                
                CreateFacebookUserRequest returnValue = new CreateFacebookUserRequest();
                returnValue.FirstName = me.first_name;
                returnValue.LastName = me.last_name;
                returnValue.LocalTime = Convert.ToInt32(me.timezone);
                returnValue.Email = me.email;
                returnValue.FacebookUserId = me.id.ToString();
                if (returnValue.Email.IsNull())
                {
                    returnValue.Email = returnValue.FacebookUserId + "@facebook.rombe.me";
                }
                

                if (me.gender == GenderTypeEnum.Male.ToString().ToLower())
                {
                    returnValue.GenderType = GenderTypeEnum.Male;
                }
                else
                {
                    returnValue.GenderType = GenderTypeEnum.Female;
                }
                
                if (me.birthday != null)
                {
                    returnValue.BirthDate = DateTime.Parse(me.birthday);
                }
               
                returnValue.UserDeviceObject = request.UserDeviceObject;
                returnValue.CampaignSource = request.CampaignSource;

                return returnValue;
            }
            catch (FacebookOAuthException)
            {
                return null;
            }
            catch(Exception)
            {
                throw;
            }
        }
        

        public String GetFacebookId(String facebookToken)
        {
            try
            {
                FacebookClient facebookClient = new FacebookClient(facebookToken);

                dynamic me = facebookClient.Get("/me");
                return me.id.ToString();
            }
            catch (FacebookOAuthException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion public methods

    }

}
