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
        private Queue<IPlayer> activePlayerQueue;

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
        protected void raiseNextBettingRound(PokerGameEventArgs e)
        {
            var handler = NextBettingRound;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<PokerPlayerEventArgs> NextPlayer;
        protected void raiseNextPlayer(PokerPlayerEventArgs e)
        {
            var handler = NextPlayer;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public PokerGame(IBetManager betManager)
        {
            this.betManager = betManager;
            this.players = new List<IPlayer>();
            this.stage = PokerGameStage.Inactive;
            this.activePlayerQueue = new Queue<IPlayer>();
        }

        public void AddPlayer(IPlayer player)
        {
            player.Participation = PlayerParticipation.Inactive;
            players.Add(player);
        }

        public void RemovePlayer(IPlayer player)
        {
            if (player.Participation != PlayerParticipation.Inactive)
                throw new PokerException("Players can only be removed from the game if they're inactive");

            players.Remove(player);
        }

        void betsAllowed()
        {
            if (stage != PokerGameStage.Blinds &&
                stage != PokerGameStage.Flop &&
                stage != PokerGameStage.Turn &&
                stage != PokerGameStage.River)
                throw new PokerStageException("Bets are only allowed in blinds, flop, turn, and river stages");
        }

        public void Fold()
        {
            betsAllowed();
            currentPlayer.Fold();
            nextPlayer();
        }

        public void PlaceBet(int amount)
        {
            betsAllowed();
            betManager.PlaceBet(currentPlayer, amount);
            nextPlayer();
        }

        public void NewGame()
        {
            // Check if we can start a new game
            if (this.stage != PokerGameStage.Finished && this.stage != PokerGameStage.Inactive)
                throw new PokerStageException("Game needs to be either in 'Inactive' or 'Finished' stage to start new game");

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

            // Advance to blinds stage
            this.stage = PokerGameStage.Blinds;

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
            // Check if any players are in the queue
            // If there are, select the first one
            if (activePlayerQueue.Count > 0)
            {
                this.currentPlayer = activePlayerQueue.Dequeue();
                return;
            }

            // If there are no more players left in the queue
            // Check if the betting round is over
            bool roundOver = betManager.BettingRoundOver();

            if (roundOver)
            {
                // If the round is over => next round
                this.nextRound();
            }
            else
            {
                // If the round is not over, we should determine which
                // players are active, and pick the first one
                determinePlayerQueue();
                this.currentPlayer = activePlayerQueue.Dequeue();
            }
        }

        internal void determinePlayerQueue()
        {
            // In a poker game, the first player to decide is the player after the big blind player
            int bigBlindPlayerIndex = Players.IndexOf(BigBlindPlayer);
            int i = bigBlindPlayerIndex;

            do
            {
                i++;

                if (i == Players.Count) i = 0;

                if (Players[i].Participation == PlayerParticipation.Active)
                    activePlayerQueue.Enqueue(Players[i]);

            } while (i != bigBlindPlayerIndex);
        }

        internal void nextRound()
        {
            switch (stage)
            {
                case PokerGameStage.Blinds:
                    stage = PokerGameStage.Flop;
                    break;
                case PokerGameStage.Flop:
                    stage = PokerGameStage.Turn;
                    break;
                case PokerGameStage.Turn:
                    stage = PokerGameStage.River;
                    break;
                case PokerGameStage.River:
                    stage = PokerGameStage.Showdown;
                    break;
            }
            if (stage != PokerGameStage.Showdown) nextPlayer();
        }

        public bool PlayersWin(IList<IPlayer> winners)
        {
            if (stage != PokerGameStage.Showdown)
                throw new PokerStageException("Winners can only be chosen in showdown stage");

            return betManager.CalculateWinners(winners);
        }

        public bool PlayerWins(IPlayer winner)
        {
            if (stage != PokerGameStage.Showdown)
                throw new PokerStageException("Winners can only be chosen in showdown stage");

            return PlayersWin(new List<IPlayer>(){ winner });
        }
    }
}
