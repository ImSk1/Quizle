using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Exceptions
{
    public class InvalidApiResponseException : Exception
    {
        public InvalidApiResponseException() : base()
        {

        }
        public InvalidApiResponseException(string message) : base(message)
        {

        }
    }
}
