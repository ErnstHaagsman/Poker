using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core.Interfaces;

namespace Poker.Core
{
    class PokerGameEventArgs : EventArgs
    {
        public PokerGameStage Stage;
    }

    class PokerPlayerEventArgs : EventArgs
    {
        public IPlayer Player;
    }
}
