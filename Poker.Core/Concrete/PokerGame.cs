using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core;
using Poker.Core.Interfaces;

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
            this.stage = PokerGameStage.Inactive;
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
            // Check if we can start a new game
            if (this.stage != PokerGameStage.Finished && this.stage != PokerGameStage.Inactive)
                throw new PokerException("Game needs to be either in 'Inactive' or 'Finished' stage to start new game");

            if (players.Count(x => x.Balance > 0) < 2)
                throw new PokerException("At least two players should have positive balance to start a game");

            // Mark players active
            foreach (var player in players)
            {
                if (player.Balance > 0)
                    player.Participation = PlayerParticipation.Active;
                else
                    player.Participation = PlayerParticipation.Inactive;
            }

            // Arrange blinds
            smallBlindPlayer = players.NextActiveOrFirst(smallBlindPlayer);
            bigBlindPlayer = players.NextActiveOrFirst(smallBlindPlayer);

            betManager.PlaceBet(smallBlindPlayer, SmallBlind);
            betManager.PlaceBet(bigBlindPlayer, BigBlind);

            // Call nextPlayer
            nextPlayer();
        }

        internal void nextPlayer()
        {
            throw new NotImplementedException();
        }

        public bool PlayersWin(IList<IPlayer> winners)
        {
            return betManager.CalculateWinners(winners);
        }

        public bool PlayerWins(IPlayer winner)
        {
            return PlayersWin(new List<IPlayer>(){ winner });
        }
    }
}
