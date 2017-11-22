using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using RomBe.Logic.PushNotification;
using RestSharp;
using Newtonsoft.Json;
using RomBe.Entities.Enums;
using RomBe.Entities;
using RomBe.Entities.Class.Common;
using RomBe.Logic.Maintenance;
using RomBe.Logic.Timeline;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Helpers;
using System.Globalization;

namespace Tester
{
    class Program
    {
        public static void Main(string[] args)
        {
            // GetMonth(47);

            // RedisCacherHelper.Init();
            // RedisCacherHelper.Remove(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey);

            //DateTime? _childBirthDate = new DateTime(2015, 5, 19);
            //if (_childBirthDate.HasValue)
            //{
            //    double temp = Math.Truncate((double)((DateTime.Now - _childBirthDate.Value).TotalDays / 7));
            //    int value= Convert.ToInt32(temp);
            //}


            //GetTimelineContentPaginationRequest request = new GetTimelineContentPaginationRequest();
            //request.ChildId = 69;
            //request.MinWeeks = 0;
            //request.MaxWeeks = 10;

            //GetTimelineDateResponse response =new TimelineLogic().InitTimelineWithOutValidation(request);


            //PushNotificationLogic _pushNotificationLogic = new PushNotificationLogic();
            //String _deviceToken = "APA91bHTzxpUtuYEaa5g6Yz3V8XCK3QS0HM6fyx3tZ3ebf2LY3LzkjZOtGGZ2942Em4RfjkUO5aqyx2f6DEPyotDXS6rQcUu8Lz3KYqCk2B3M2VKLvkzzTXMBNDV7dEMnwH2S4Cr_E7r";
            //AndroidJsonObject _androidJsonObject = new AndroidJsonObject()
            //{
            //    Message = "Hello World!",
            //    Title = "TitleMe"
            //};


            //_pushNotificationLogic.Android(_androidJsonObject,_deviceToken);
            //_pushNotificationLogic.StopService();


            using (RombeEntities context = new RombeEntities())
            {

                {
                    new MaintenanceLogic().DeleteUser("cohenal@gmail.com");
                }
            }

            //    int counter = 10000;
            //    using (RombeEntities context = new RombeEntities())
            //    {

            //        List<RealTimeLeadingQuestion> toUpdate = context.RealTimeLeadingQuestions.OrderBy(a=>a.PeriodMax).ToList();
            //        Console.WriteLine("number of items: ", toUpdate.Count);
            //        foreach (RealTimeLeadingQuestion item in toUpdate)
            //        {
            //            item.UniqueId = counter;
            //            counter++;
            //        }
            //        context.SaveChanges();
            //    }

            //    //Console.WriteLine("Counter value: ", counter);


        }

        static void GetMonth(int Week)
        {
            Dictionary<int, List<int>> test = new Dictionary<int, List<int>>();
            int numberOfWeeks =0;
            int month = 1;
            for (int i = 0; i < 48; i++)
            {

                if(numberOfWeeks == 4)
                {
                    numberOfWeeks = 0;
                    month++;
                    i--;
                }
                else
                {
                    if(test.ContainsKey(month))
                    {
                        test[month].Add(i);
                        numberOfWeeks++;
                    }
                    else
                    {
                        test.Add(month, new List<int>() { i });
                        numberOfWeeks++;
                    }
                }
            }
            






        }
    }
}
