using RomBe.Entities;
using RomBe.Entities.Class.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Entities.DAL;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using RomBe.Entities.Class.Request;

namespace RomBe.Logic.Config
{
    public class ConfigLogic
    {

        #region public methods
        public GetConfigResponse GetConfig(LanguagesEnum language)
        {
            try
            {
                GetConfigResponse response;
                if (RedisCacherHelper.Exists(SystemConfigurationHelper.ConfigurationnCahceKey))
                {
                    response = RedisCacherHelper.Get<GetConfigResponse>(SystemConfigurationHelper.ConfigurationnCahceKey);
                }
                else
                {
                    response = SetConifgInMemoryCache();
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IsConfigUpdatedResponse IsConfigUpdated(int currentUpdateValue)
        {
            int lastUpdateValue = new ConfigDAL().IsConfigUpdated(currentUpdateValue);
            if (lastUpdateValue > currentUpdateValue)
            {
                RedisCacherHelper.Remove(SystemConfigurationHelper.ConfigurationnCahceKey);
                SetConifgInMemoryCache();
                return new IsConfigUpdatedResponse()
                {
                    LastUpdateCheck = lastUpdateValue,
                    NeedToUpdate = true
                };
            }
            return new IsConfigUpdatedResponse()
            {
                LastUpdateCheck = currentUpdateValue,
                NeedToUpdate = false
            };

        }
        public IsVersionValidResponse IsVersionValid(OperatingSystemTypeEnum operatingSystemType, string currentVersion)
        {
            return new ConfigDAL().IsVersionValid(operatingSystemType, currentVersion);
        }

        public GetConfigResponse SetConifgInMemoryCache()
        {
            GetConfigResponse response;
            if (!RedisCacherHelper.Exists(SystemConfigurationHelper.ConfigurationnCahceKey))
            {
                response = new GetConfigResponse();
                response.SystemMessagesList = BuildSystemMessagesList((int)RomBe.Entities.Enums.LanguagesEnum.English);
                bool isAddedSucceeded = RedisCacherHelper.Add(SystemConfigurationHelper.ConfigurationnCahceKey, response, new DateTimeOffset(DateTime.Now.AddDays(SystemConfigurationHelper.CacheExpirationInDays)));

                return response;
            }
            else
            {
                return RedisCacherHelper.Get<GetConfigResponse>(SystemConfigurationHelper.ConfigurationnCahceKey);
            }

        }

        #endregion public methods

        #region private methods
        private List<SystemMessageObject> BuildSystemMessagesList(int languageId)
        {
            List<SystemMessage> result = new ConfigDAL().GetSystemMessages(languageId);
            List<SystemMessageObject> list = new List<SystemMessageObject>();
            if (!result.IsNull() && result.Any())
            {
                foreach (SystemMessage item in result)
                {
                    list.Add(new SystemMessageObject()
                    {
                        Code = item.MessageCode,
                        CancelButtonText = item.CancelButtonText,
                        Content = item.MessageContent,
                        OKButtonText = item.OKButtonText,
                        Title = item.MessageTitle
                    });
                }
            }
            return list;
        }




        #endregion private methods
    }
}
