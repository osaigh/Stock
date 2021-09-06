using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.APIClient.Errors
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
