using NLog;
using NLog.Config;
using NLog.Internal;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NLogLab
{
    class Program
    {
        static void Main(string[] args)
        {
            // Config();
            Programing();
        }

        private static void Config()
        {
            var log = LogManager.GetLogger("configLog");
            log.Debug("Debug");
            log.Error("Error");
        }

        private static void Programing()
        {
            FileTarget target = new FileTarget();
            target.Layout = "${longdate} ${logger} ${message}";
            target.FileName = "${basedir}/KNLogs/${date:format=yyyyMMdd}.log";

            MailTarget mailTarget = new MailTarget();
            mailTarget.Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}";
            mailTarget.AddNewLines = true;
            mailTarget.To = "kimxinfo@gmail.com";
            //mailTarget.From = "kimxinfo@gmail.com";
            //mailTarget.SmtpAuthentication = SmtpAuthenticationMode.Basic;
            //mailTarget.SmtpPort = 587;
            //mailTarget.SmtpServer = "smtp.gmail.com";
            //mailTarget.SmtpUserName = "kimxinfo@gmail.com";
            //mailTarget.SmtpPassword = "xxx";
            //mailTarget.EnableSsl = true;
            var smtpSection = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            mailTarget.From = smtpSection.From;
            mailTarget.SmtpAuthentication = SmtpAuthenticationMode.Basic;
            mailTarget.SmtpPort = smtpSection.Network.Port;
            mailTarget.SmtpServer = smtpSection.Network.Host;
            mailTarget.SmtpUserName = smtpSection.Network.UserName;
            mailTarget.SmtpPassword = smtpSection.Network.Password;
            mailTarget.EnableSsl = smtpSection.Network.EnableSsl;
            mailTarget.Subject = "${machinename}-${event-properties:item=stage}-${longdate}";
            mailTarget.Body = "[log time]: ${longdate}${newline}[log level]: ${level:uppercase=true}${newline}[log by]: ${logger}${newline}[log message]: ${message}";

            // NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            // NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(mailTarget, LogLevel.Error);
            var fileRule = new LoggingRule("SysCore", LogLevel.Trace, target);
            LogManager.Configuration.AddTarget("file", target);
            LogManager.Configuration.LoggingRules.Add(fileRule);

            var mailRule = new LoggingRule("SysCore", LogLevel.Error, mailTarget);
            LogManager.Configuration.AddTarget("mail", mailTarget);
            LogManager.Configuration.LoggingRules.Add(mailRule);
            LogManager.ReconfigExistingLoggers();//覆寫預設組態
            var log = LogManager.GetLogger("SysCore");

            // log.Error("Kim Trace");

            LogEventInfo myEvent = new LogEventInfo(LogLevel.Error, log.Name, "My debug message");
            myEvent.Properties.Add("stage", "QAS");

            log.Log(myEvent);
        }
    }
}
