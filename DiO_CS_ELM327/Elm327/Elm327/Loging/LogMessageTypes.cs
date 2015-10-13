using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loging
{
    /// <summary>
    /// Describe the LOG messages types.
    /// </summary>
    public enum LogMessageTypes
    {
        /// <summary>
        /// Everything is OK
        /// </summary>
        Ok,
        /// <summary>
        /// You must pay atention to this message.
        /// </summary>
        Warning,
        /// <summary>
        /// Somthing happend that it must not be happening.
        /// </summary>
        Error,
        /// <summary>
        /// Information for some event, proces or control message.
        /// </summary>
        Info
    }
}
