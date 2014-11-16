using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Poker.Core;
using Poker.Core.Interfaces;
using Poker.Core.Concrete;

using Xunit;

namespace Poker.Tests
{
    public class PlayerTests
    {
        /// <summary>
        /// Factory method
        /// </summary>
        /// <returns></returns>
        IPlayer getPlayer() 
        {
            return new Player();
        }

        [Fact]
        public void Player_PlaceBet()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;

            // Act
            player.PlaceBet(35);

            // Assert
            Assert.Equal(65, player.Balance);
            Assert.Equal(35, player.Bet);
        }

        [Fact]
        public void Player_PlaceBetAllInOverBalance()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;

            // Act
            player.PlaceBet(150);

            // Assert
            Assert.Equal(0, player.Balance);
            Assert.Equal(100, player.Bet);
            Assert.Equal(PlayerParticipation.AllIn, player.Participation);
        }

        [Fact]
        public void Player_PlaceBetAllInAtBalance()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;

            // Act
            player.PlaceBet(100);

            // Assert
            Assert.Equal(0, player.Balance);
            Assert.Equal(100, player.Bet);
            Assert.Equal(PlayerParticipation.AllIn, player.Participation);
        }

        [Fact]
        public void Player_Win()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;
            player.PlaceBet(30);

            // Act
            player.Win(30);

            // Assert
            Assert.Equal(130, player.Balance);
            Assert.Equal(0, player.Bet);
        }

        [Fact]
        public void Player_LoseAll()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;
            player.PlaceBet(30);

            // Act
            player.Lose();

            // Assert
            Assert.Equal(70, player.Balance);
            Assert.Equal(0, player.Bet);
        }

        [Fact]
        public void Player_LosePart()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;
            player.PlaceBet(30);

            // Act
            player.Lose(15);

            // Assert
            Assert.Equal(70, player.Balance);
            Assert.Equal(15, player.Bet);
        }

        [Fact]
        public void Player_Fold()
        {
            // Arrange
            IPlayer player = getPlayer();
            player.Balance = 100;
            player.PlaceBet(30);

            // Act
            player.Fold();

            // Assert
            Assert.Equal(PlayerParticipation.Inactive, player.Participation);
        }
    }
}
