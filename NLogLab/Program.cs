using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Config();
            // Programing();
        }

        private static void Config()
        {
            var log = LogManager.GetCurrentClassLogger();
            log.Debug("Debug");
            log.Error("Error");
        }

        private static void Programing()
        {
            FileTarget target = new FileTarget();
            target.Layout = "${longdate} ${logger} ${message}";
            target.FileName = "${basedir}/KNLogs/${date:format=yyyyMMdd}.log";
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            var log = LogManager.GetLogger("log");
            log.Debug("Kim Trace");
        }
    }
}
