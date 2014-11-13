using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poker.Core;
using Poker.Core.Interfaces;

namespace Poker.Core.Concrete
{
    public class Player : IPlayer
    {
        public int Balance { get; set; }

        private int bet;
        public int Bet
        {
            get { return bet; }
        }

        public string Name { get; set; }

        public PlayerParticipation Participation { get; set; }

        public void Fold()
        {
            this.Participation = PlayerParticipation.Inactive;
        }

        public void Lose()
        {
            this.bet = 0;
        }

        public void Lose(int amount)
        {
            if (amount > this.bet)
                throw new ArgumentOutOfRangeException("amount","Amount larger than bet");

            this.bet -= amount;
        }

        public void PlaceBet(int amount)
        {
            if (amount >= this.Balance)
            {
                this.bet = this.Balance;
                this.Balance = 0;
                this.Participation = PlayerParticipation.AllIn;
                return;
            }

            this.bet += amount;
            this.Balance -= amount;
        }

        public void Win(int amount)
        {
            this.Balance += this.bet;
            this.bet = 0;
            this.Balance += amount;
        }
    }
}
