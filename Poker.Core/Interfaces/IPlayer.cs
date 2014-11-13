using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core;

namespace Poker.Core.Interfaces
{
    interface IPlayer
    {
        // Properties

        /// <summary>
        /// The current amount of money the player has
        /// </summary>
        int Balance { get; set; }
        
        /// <summary>
        /// The bet the player has made in the current round
        /// </summary>
        int Bet { get; }

        /// <summary>
        /// The player's name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The player's current participation state
        ///  - Active   The player is able to bet
        ///  - AllIn    The player has gone all in
        ///  - Inactive The player has folded
        /// </summary>
        PlayerParticipation Participation { get; set; }

        // Methods

        /// <summary>
        /// Folds the player. This changes the player's status to
        /// inactive
        /// </summary>
        void Fold();

        /// <summary>
        /// Reset the player's bet to 0
        /// </summary>
        void Lose();

        /// <summary>
        /// Decrease the player's bet by the specified amount
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="ArgumentOutOfRangeException">When 'amount' is larger than the player's bet</exception>
        void Lose(int amount);

        /// <summary>
        /// Increase the player's bet by the specified amount, 
        /// the bet will be transferred from the player's balance.
        /// If the requested bet is larger than, or equal to, the player's balance,
        /// the full balance will be transferred to bet, and the player's
        /// status will be set to 'AllIn'
        /// </summary>
        /// <param name="amount"></param>
        void PlaceBet(int amount);

        /// <summary>
        /// Transfers the bet (if any) to the player's balance,
        /// and adds the specified amount to the player's balance
        /// as well
        /// </summary>
        /// <param name="amount"></param>
        void Win(int amount);
    }
}
