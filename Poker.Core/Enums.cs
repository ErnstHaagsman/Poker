using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core
{
    public enum PokerGameStage
    {
        Blinds,
        Flop,
        Turn,
        River,
        Showdown,
        Finished
    }

    public enum PlayerParticipation
    {
        Active,
        AllIn,
        Inactive
    }
}
