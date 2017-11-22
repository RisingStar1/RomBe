using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using RomBe.Entities.Class.Common;
using RomBe.Entities.DAL;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.PushNotification
{
    public class PushNotificationLogic
    {
        private PushBroker _pushBroker = null;
        private PushBroker PushBroker
        {
            get
            {
                if (_pushBroker == null)
                {
                    _pushBroker = new PushBroker();
                    Init();
                }

                return _pushBroker;
            }
        }
        private void Init()
        {
            EventsInit();
            //AppleInit();
            AndroidInit();
        }

        private void EventsInit()
        {
            //Wire up the events for all the services that the broker registers
            PushBroker.OnNotificationSent += NotificationSent;
            PushBroker.OnChannelException += ChannelException;
            PushBroker.OnServiceException += ServiceException;
            PushBroker.OnNotificationFailed += NotificationFailed;
            PushBroker.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            PushBroker.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            PushBroker.OnChannelCreated += ChannelCreated;
            PushBroker.OnChannelDestroyed += ChannelDestroyed;
            PushBroker.OnNotificationRequeue += NotificationRequeue;
        }

        private void AppleInit()
        {
            bool production = SystemConfigurationHelper.IsProduction;

            String p12File = string.Empty;
            string password = SystemConfigurationHelper.IphonePushPassword;

            if (production)
            {
                p12File = SystemConfigurationHelper.IphonePushCertificate;
            }
            else
            {
                p12File = SystemConfigurationHelper.IphonePushCertificateDev;
            }

            var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File));
            //IMPORTANT: If you are using a Development provisioning Profile, you must use the Sandbox push notification server 
            //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as 'false')
            //  If you are using an AdHoc or AppStore provisioning profile, you must use the Production push notification server
            //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 'true')

            PushBroker.RegisterAppleService(new ApplePushChannelSettings(production, appleCert, "password"));
            //Fluent construction of an iOS notification
            //IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets generated within your iOS app itself when the Application Delegate
            //  for registered for remote notifications is called, and the device token is passed back to you
        }

        private void AndroidInit()
        {
            String androidAPIKey = SystemConfigurationHelper.AndroidAPIKey;
            //---------------------------
            // ANDROID GCM NOTIFICATIONS
            //---------------------------
            //Configure and start Android GCM
            //IMPORTANT: The API KEY comes from your Google APIs Console App, under the API Access section, 
            //  by choosing 'Create new Server key...'
            //  You must ensure the 'Google Cloud Messaging for Android' service is enabled in your APIs Console
            PushBroker.RegisterGcmService(new GcmPushChannelSettings(androidAPIKey));
        }

        public void StopService()
        {
            PushBroker.StopAllServices();
        }
        public void Apple(String deviceToken, Dictionary<String, String> payLoadCustomItems)
        {

            

                AppleNotificationPayload appleNotificationPayload = new AppleNotificationPayload();
                foreach (var item in payLoadCustomItems)
                {
                    appleNotificationPayload.AddCustom(item.Key, item.Value);
                    //appleNotificationPayload.Alert.Body = message;
                }

                PushBroker.QueueNotification(new AppleNotification()
                                          .ForDeviceToken(deviceToken)
                                          .WithPayload(appleNotificationPayload)
                                          .WithSound("sound.caf"));
            

        }

        public void Android(AndroidJsonObject notification, String deviceToken)
        {
           
            String json = JsonConvert.SerializeObject(notification);


            //Fluent construction of an Android GCM Notification
            //IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
            PushBroker.QueueNotification(new GcmNotification().ForDeviceRegistrationId(deviceToken).WithJson(json));



        }

        #region Events
        void NotificationRequeue(object sender, NotificationRequeueEventArgs e)
        {
            //_log.Debug("Requeue: " + sender + " -> " + e.Notification);
        }
        void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Currently this event will only ever happen for Android GCM
            //Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
        }

        void NotificationSent(object sender, INotification notification)
        {

            //Console.WriteLine("Sent: " + sender + " -> " + notification);
        }

        void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {

            GcmMessageTransportException tempExeption= (GcmMessageTransportException)notificationFailureException;
            //tempExeption.Response.Message.RegistrationIds
            new NotificationDAL().MarkNotificaionAsFaild(tempExeption.Response.Message.RegistrationIds, tempExeption.Message);
            LoggerHelper.Info(tempExeption);
            //Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
        }

        void ChannelException(object sender, IPushChannel channel, Exception exception)
        {
            //Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        }

        void ServiceException(object sender, Exception exception)
        {
            //Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        }

        void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        {
            //Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
        }

        void ChannelDestroyed(object sender)
        {
            //Console.WriteLine("Channel Destroyed for: " + sender);
        }

        void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Console.WriteLine("Channel Created for: " + sender);
        }
        
        #endregion Events
    }
    
}
