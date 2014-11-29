using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core
{
    public class PokerException : Exception
    {
        public PokerException()
        {
        }

        public PokerException(string message) :
            base(message)
        {
        }
    }
}
