using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Poker.Core;
using Poker.Core.Interfaces;
using Poker.Core.Concrete;

using Xunit;

namespace Poker.Tests.PokerGameTests
{
    public class RegularGame
    {
        /// <summary>
        /// Factory method
        /// </summary>
        /// <returns></returns>
        IPokerGame getGame()
        {
            return new PokerGame();
        }

        public RegularGame()
        {
        }   

    }
}
