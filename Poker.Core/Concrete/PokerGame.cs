using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core;
using Poker.Core.Interfaces;
using Poker.Core.Utilities;

namespace Poker.Core.Concrete
{
    class PokerGame : IPokerGame
    {
        private IBetManager betManager;

        public IBetManager BetManager
        {
            get { return betManager; }
        }

        public int BigBlind
        {
            get
            {
                return betManager.BigBlind;
            }
            set
            {
                betManager.BigBlind = value;
            }
        }

        IPlayer bigBlindPlayer;
        public IPlayer BigBlindPlayer
        {
            get { return bigBlindPlayer; }
        }

        public int SmallBlind
        {
            get
            {
                return betManager.SmallBlind;
            }
            set
            {
                betManager.SmallBlind = value;
            }
        }

        IPlayer smallBlindPlayer;
        public IPlayer SmallBlindPlayer
        {
            get { return smallBlindPlayer; }
        }

        public int CurrentBet
        {
            get { return betManager.CurrentBet; }
        }

        private IPlayer currentPlayer;
        public IPlayer CurrentPlayer
        {
            get { return currentPlayer; }
        }

        private List<IPlayer> players;
        public IList<IPlayer> Players
        {
            get { return players; }
        }

        PokerGameStage stage;
        public PokerGameStage Stage
        {
            get { return stage; }
        }

        public event EventHandler<PokerGameEventArgs> NextBettingRound;

        public event EventHandler<PokerPlayerEventArgs> NextPlayer;

        public PokerGame(IBetManager betManager)
        {
            this.betManager = betManager;
            this.players = new List<IPlayer>();
        }

        public void AddPlayer(IPlayer player)
        {
            player.Participation = PlayerParticipation.Inactive;
            players.Add(player);
        }

        public void RemovePlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Fold()
        {
            currentPlayer.Fold();
        }

        public void PlaceBet(int amount)
        {
            betManager.PlaceBet(currentPlayer, amount);
        }

        public void NewGame()
        {
            // Mark players active
            foreach (var player in players)
            {
                if (player.Balance > 0)
                    player.Participation = PlayerParticipation.Active;
                else
                    player.Participation = PlayerParticipation.Inactive;
            }

            // Arrange blinds
            smallBlindPlayer = players.NextAfterOrFirst(smallBlindPlayer);
            bigBlindPlayer = players.NextAfterOrFirst(bigBlindPlayer);

            betManager.PlaceBet(smallBlindPlayer, SmallBlind);
            betManager.PlaceBet(bigBlindPlayer, BigBlind);

            // Call nextPlayer
            nextPlayer();
        }

        internal void nextPlayer()
        {
            throw new NotImplementedException();
        }

        public bool PlayersWin(List<IPlayer> winners)
        {
            return betManager.CalculateWinners(winners);
        }

        public bool PlayerWins(IPlayer winner)
        {
            return PlayersWin(new List<IPlayer>(){ winner });
        }
    }
}
