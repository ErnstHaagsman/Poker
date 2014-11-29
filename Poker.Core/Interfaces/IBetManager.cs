using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Interfaces
{
    public interface IBetManager
    {
        /// <summary>
        /// Current big blind
        /// </summary>
        int BigBlind { get; set; }

        /// <summary>
        /// Current small blind
        /// </summary>
        int SmallBlind { get; set; }

        /// <summary>
        /// Current bet
        /// </summary>
        int CurrentBet { get; }

        /// <summary>
        /// Is the betting round over, and can the game advance to the next stage?
        /// </summary>
        /// <returns></returns>
        bool BettingRoundOver();

        /// <summary>
        /// Transfers the appropriate amount to the winning players. Returns false
        /// if a side pot needs to be resolved
        /// </summary>
        /// <param name="players">Players with winning cards, more than one for split pot situations</param>
        /// <returns></returns>
        bool CalculateWinners(IList<IPlayer> players);

        /// <summary>
        /// Bet the specified amount for the specified player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount"></param>
        void PlaceBet(IPlayer player, int amount);
    }
}
