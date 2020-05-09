using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class ExceptionManagementHelper : Exception
    {
        public ExceptionManagementHelper(string message):base(message)
        {
        }
        public ExceptionManagementHelper(string message,Exception innerException) : base(message,innerException)
        {
        }
    }
}
