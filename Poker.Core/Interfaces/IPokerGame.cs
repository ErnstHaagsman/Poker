using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core;

namespace Poker.Core.Interfaces
{
    interface IPokerGame
    {
        // Properties

        /// <summary>
        /// The current big blind
        /// </summary>
        int BigBlind { get; set; }

        /// <summary>
        /// The player who has to bet the big blind this game
        /// </summary>
        IPlayer BigBlindPlayer { get; }

        /// <summary>
        /// The current small blind
        /// </summary>
        int SmallBlind { get; set; }

        /// <summary>
        /// The player who has to bet the small blind this game
        /// </summary>
        IPlayer SmallBlindPlayer { get; }

        /// <summary>
        /// The current bet which has to be matched to stay in
        /// the game
        /// </summary>
        int CurrentBet { get; }

        /// <summary>
        /// The player who may currently bet or fold
        /// </summary>
        IPlayer CurrentPlayer { get; }

        /// <summary>
        /// The players in the game
        /// </summary>
        List<IPlayer> Players { get; }

        /// <summary>
        /// The current stage of the game
        /// </summary>
        PokerGameStage Stage { get; }

        /// <summary>
        /// IBetManager instance used
        /// </summary>
        IBetManager BetManager { get; }

        // Events

        /// <summary>
        /// Raised when the game enters a new stage (e.g. river)
        /// </summary>
        event EventHandler<PokerGameEventArgs> NextBettingRound;

        /// <summary>
        /// Raised when CurrentPlayer changes
        /// </summary>
        event EventHandler<PokerPlayerEventArgs> NextPlayer;

        // Methods

        /// <summary>
        /// Adds a new player to the game, will be
        /// added as inactive
        /// </summary>
        /// <param name="player"></param>
        void AddPlayer(IPlayer player);

        /// <summary>
        /// Removes a player from the game, only possible
        /// if the player is inactive
        /// </summary>
        /// <param name="player"></param>
        void RemovePlayer(IPlayer player);

        /// <summary>
        /// Folds CurrentPlayer's hand
        /// </summary>
        void Fold();

        /// <summary>
        /// Places a bet for CurrentPlayer
        /// </summary>
        /// <param name="amount"></param>
        void PlaceBet(int amount);

        /// <summary>
        /// Starts a new game, only possible when the current
        /// stage is 'Finished'
        /// </summary>
        void NewGame();

        /// <summary>
        /// Determines winnings and losses when multiple 
        /// players win (a split pot scenario)
        /// </summary>
        /// <param name="winners"></param>
        /// <returns>False if one of the winners was all-in and a side pot needs to be resolved</returns>
        bool PlayersWin(List<IPlayer> winners);

        /// <summary>
        /// Determines winnings and losses
        /// </summary>
        /// <param name="winner"></param>
        /// <returns>False if one of the winners was all-in and a side pot needs to be resolved</returns>
        bool PlayerWins(IPlayer winner);
    }
}
