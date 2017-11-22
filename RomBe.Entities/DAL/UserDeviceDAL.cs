using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using RomBe.Entities.Enums;

namespace RomBe.Entities.DAL
{
    public class UserDeviceDAL
    {
        public async Task<Boolean> CreateUserDeviceAsync(UserDeviceObject userDeviceRequest, int userId)
        {
            try
            {
                if (userDeviceRequest.DeviceId.IsNull())
                    return false;

                using (RombeEntities context = new RombeEntities())
                {
                    User _currentUser = context.Users.Where(u => u.UserId == userId).FirstOrDefault();
                    if (!_currentUser.IsNull())
                    {


                        int result;
                        UserDevice userDeviceResult = context.UserDevices.Where(u => u.UserId == userId &&
                            u.DeviceId == userDeviceRequest.DeviceId).FirstOrDefault();

                        //update the row if the data exist
                        if (!userDeviceResult.IsNull())
                        {
                            UpdateExisting(userDeviceRequest, userDeviceResult);
                        }
                        //create new row if the data is not exist
                        else
                        {
                            if (context.Users.Where(u => u.UserId == userId).Any())
                            {
                                UserDevice newUserDevice = Create(userDeviceRequest, userId);
                                context.UserDevices.Add(newUserDevice);
                            }
                        }

                        _currentUser.SendNotifications = true;

                        result = await context.SaveChangesAsync().ConfigureAwait(false);
                        return Convert.ToBoolean(result);
                    }
                    else return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private UserDevice Create(UserDeviceObject userDeviceRequest, int userId)
        {
            UserDevice newUserDevice = new UserDevice();
            newUserDevice.UserId = userId;
            newUserDevice.DeviceId = !userDeviceRequest.DeviceId.IsNull() ? userDeviceRequest.DeviceId : string.Empty;
            newUserDevice.DeviceName = !userDeviceRequest.DeviceName.IsNull() ? userDeviceRequest.DeviceName : string.Empty;
            newUserDevice.OsType = userDeviceRequest.DeviceOS > 0 ? (int)userDeviceRequest.DeviceOS : 0;
            newUserDevice.OsVersion = !userDeviceRequest.DeviceOsVersion.IsNull() ? userDeviceRequest.DeviceOsVersion : string.Empty;
            newUserDevice.PushToken = !userDeviceRequest.PushToken.IsNull() ? userDeviceRequest.PushToken : string.Empty;

            newUserDevice.UpdateDate = DateTime.Now;
            newUserDevice.InsertDate = DateTime.Now;
            return newUserDevice;
        }

        private void UpdateExisting(UserDeviceObject userDevice, UserDevice userDeviceResult)
        {
            userDeviceResult.DeviceId = !String.IsNullOrEmpty(userDevice.DeviceId) ? userDevice.DeviceId : userDeviceResult.DeviceId;
            userDeviceResult.DeviceName = !String.IsNullOrEmpty(userDevice.DeviceName) ? userDevice.DeviceName : userDeviceResult.DeviceName;
            userDeviceResult.OsType = userDevice.DeviceOS > 0 ? (int)userDevice.DeviceOS : userDeviceResult.OsType;
            userDeviceResult.OsVersion = !String.IsNullOrEmpty(userDevice.DeviceOsVersion) ? userDevice.DeviceOsVersion : userDeviceResult.OsVersion;
            userDeviceResult.PushToken = !String.IsNullOrEmpty(userDevice.PushToken) ? userDevice.PushToken : userDeviceResult.PushToken;
            userDeviceResult.UpdateDate = DateTime.Now;
        }
    }
}
