using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Exceptions
{
    public class CannotBuyBadgeException : Exception
    {
        public CannotBuyBadgeException() : base()
        {

        }
        public CannotBuyBadgeException(string message) : base(message)
        {

        }
    }
       
}
