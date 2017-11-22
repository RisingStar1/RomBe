using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using RomBe.Entities;
using System.Data.Entity;

namespace RomBe.SendEmailService
{
    public partial class EmailSendService : ServiceBase
    {
        Timer timer;
        public EmailSendService()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 30000; // 60 seconds 60000
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();


        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            //eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);

          LoggerHelper.Logger.Info(args.SignalTime.ToString());

            //using (RombeEntities context = new RombeEntities())
            //{
            //    Log newLog = new Log();
            //    newLog.Date = DateTime.Now;
            //    newLog.Exception = "adsadasd";
            //    newLog.Thread = "6";
            //    newLog.Level = "ERROR";
            //    newLog.Logger = "aRomBe";
            //    newLog.Message = "asdsada";
            //    newLog.UserId = 0;

            //    context.Logs.Add(newLog);
            //    context.SaveChanges();
            //}
        }
    }
}
