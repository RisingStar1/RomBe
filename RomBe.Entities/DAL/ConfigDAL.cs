using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.DAL
{
    public class ConfigDAL
    {
        private const string CONFIG_UPDATED = "IsConfigUpdated";
        public List<SystemMessage> GetSystemMessages(int languageId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    return context.SystemMessages.Where(s => s.LanguageId == languageId && s.SystemMessageTypeId != (int)SysteMessagesTypeEnum.Email).ToList();
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
        }

        public int IsConfigUpdated(int currentUpdateValue)
        {
            using (RombeEntities context = new RombeEntities())
            {
                GlobalParameter isConfigUpdated = context.GlobalParameters.Where(g => g.ParameterName == CONFIG_UPDATED).FirstOrDefault();
                if (isConfigUpdated != null)
                {
                    if (isConfigUpdated.ParameterValueInt > currentUpdateValue)
                    {
                        return  isConfigUpdated.ParameterValueInt.Value;
                    }
                }
                return currentUpdateValue;
            }
        }
        public IsVersionValidResponse IsVersionValid(OperatingSystemTypeEnum operatingSystemType, string currentVersion)
        {
            using (RombeEntities context = new RombeEntities())
            {
                int? result = context.IsVersionValid(operatingSystemType.ToString(), currentVersion).FirstOrDefault();
                return new IsVersionValidResponse()
                {
                    IsVersionValid = Convert.ToBoolean(result).ToString()
                };
            }
        }
    }
}
