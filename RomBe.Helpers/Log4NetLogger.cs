using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Log4NetCore = log4net.Core;

namespace RomBe.Helpers
{
    //public class Log4NetLogger : ILogger
    //{
    //    private readonly log4net.ILog _logger;

    //    #region public methods
    //    public Log4NetLogger()
    //    {
    //        _logger = log4net.LogManager.GetLogger(GetLoggerName());
    //        log4net.Config.XmlConfigurator.Configure();
    //    }

    //    public void Debug(string message, int userId = 0)
    //    {
    //        WriteLog(message, userId, Log4NetCore.Level.Debug);
    //    }

    //    public void Info(string message, int userId = 0)
    //    {
    //        WriteLog(message, userId, Log4NetCore.Level.Info);
    //    }

    //    public void Warning(string message, int userId = 0)
    //    {
    //        WriteLog(message, userId, Log4NetCore.Level.Warn);
    //    }

    //    public void Error(string message, Exception exception, int userId = 0)
    //    {
    //        WriteLog(message, userId, Log4NetCore.Level.Error, exception);
    //    }

    //    #endregion public methods

    //    #region private methods
    //    private void WriteLog(string message, int userId, Log4NetCore.Level level, Exception exception = null)
    //    {
    //        var log = new Log4NetCore.LoggingEvent(GetType(), _logger.Logger.Repository, _logger.Logger.Name, level, message, exception);
    //        log.Properties["UserId"] = userId;
    //        _logger.Logger.Log(log);
    //    }

    //    private string GetLoggerName()
    //    {
    //        //string result = ConfigurationManager.AppSettings["LOGGER_NAME"];

    //        //if (string.IsNullOrEmpty(result))
    //        //{
    //        //    result = "rombe";
    //        //}

    //        //return result;
    //        return "RomBe";

    //    }

    //    #endregion private methods
    //}

    //public interface ILogger
    //{
    //    void Debug(string message, int userId = 0);

    //    void Info(string message, int userId = 0);

    //    void Warning(string message, int userId = 0);

    //    void Error(string message, Exception exception, int userId = 0);
    //}
}
