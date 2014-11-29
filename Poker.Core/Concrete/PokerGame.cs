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

        public int BigBlind
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IPlayer BigBlindPlayer
        {
            get { throw new NotImplementedException(); }
        }

        public int SmallBlind
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IPlayer SmallBlindPlayer
        {
            get { throw new NotImplementedException(); }
        }

        public int CurrentBet
        {
            get { throw new NotImplementedException(); }
        }

        public IPlayer CurrentPlayer
        {
            get { throw new NotImplementedException(); }
        }

        public List<IPlayer> Players
        {
            get { throw new NotImplementedException(); }
        }

        public PokerGameStage Stage
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<PokerGameEventArgs> NextBettingRound;

        public event EventHandler<PokerPlayerEventArgs> NextPlayer;

        public void AddPlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Fold()
        {
            throw new NotImplementedException();
        }

        public void PlaceBet(int amount)
        {
            throw new NotImplementedException();
        }

        public void NewGame()
        {
            throw new NotImplementedException();
        }

        public bool PlayersWin(List<IPlayer> winners)
        {
            throw new NotImplementedException();
        }

        public bool PlayerWins(IPlayer winner)
        {
            throw new NotImplementedException();
        }


        public IBetManager BetManager
        {
            get { throw new NotImplementedException(); }
        }
    }
}
