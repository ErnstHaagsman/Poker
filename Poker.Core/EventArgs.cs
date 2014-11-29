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

        public PokerGameEventArgs(PokerGameStage stage)
        {
            this.Stage = stage;
        }
    }

    class PokerPlayerEventArgs : EventArgs
    {
        public IPlayer Player;

        public PokerPlayerEventArgs(IPlayer player)
        {
            this.Player = player;
        }
    }
}
