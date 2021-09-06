using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.APIClient.Exceptions
{
    public class StockAPIException:Exception
    {
        #region Fields

        #endregion

        #region Properties
        public string RequestMessage { get; private set; }

        private string oldStackTrace;
        public override string StackTrace
        {
            get
            {
                if (string.IsNullOrEmpty(this.oldStackTrace))
                {
                    return base.StackTrace;
                }
                else
                {
                    return this.oldStackTrace;
                }
            }
        }
        #endregion

        #region Constructor
        public StockAPIException(string message, string requestMessage, string stackTrace) : base(message)
        {
            RequestMessage = requestMessage;
            oldStackTrace = string.IsNullOrEmpty(stackTrace) ? null : stackTrace;
        }
        #endregion
    }
}
