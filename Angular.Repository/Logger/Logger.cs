using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular.Repository.Logger
{
    public class Logger : ILogger
    {
        private ILog _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger(typeof(Logger));
        }
   
        /// <summary>
        /// log information using log4net
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(message);
            }
        }

        /// <summary>
        /// log exception using log4net
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(ex);
            }
        }


        public void LogDebug(string message)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(message);
            }
        }

        public void SetOwner(Type type)
        {
            _logger = LogManager.GetLogger(type);
        }

        public static void ConfigureLogger(string path)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }
    }
}
