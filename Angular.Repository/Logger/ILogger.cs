using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular.Repository.Logger
{
   public interface ILogger
    {
        /// <summary>
        /// Logs info messages to underlying log provider
        /// </summary>
        /// <param name="message">message that will be logged</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs Exceptions to underlying log provider
        /// </summary>
        /// <param name="ex">Exception that was raised</param>
        void LogException(Exception ex);

        /// <summary>
        /// Logs Debug messages to underlying log provider
        /// </summary>
        /// <param name="message">message that will be logged</param>
        void LogDebug(string message);

        /// <summary>
        /// Sets Logger assembly
        /// </summary>
        /// <param name="type">Logger assemly type</param>
        void SetOwner(Type type);
    }
}
