using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediBook.Domain.Exceptions
{
    public class SlotUnavailableException : Exception
    { 
        public SlotUnavailableException(string message) : base(message)
        {
        }
    }
}
