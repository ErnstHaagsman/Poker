﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Poker.Core;
using Poker.Core.Interfaces;
using Poker.Core.Concrete;

using Xunit;
using Moq;

namespace Poker.Tests.PokerGameTests
{
    public class EarlyGame
    {
        #region Test Suite Constants
        
        const int PLAYER_COUNT = 3;
        const int PLAYER_BALANCE = 3000;
        const int SMALL_BLIND = 50;
        const int BIG_BLIND = 100;
        
        #endregion


        /// <summary>
        /// Factory method
        /// </summary>
        /// <returns></returns>
        IPokerGame getGame()
        {
            betManager = new Mock<IBetManager>();
            betManager.Setup(x => x.BettingRoundOver()).Returns(false);
            betManager.SetupAllProperties();
            return new PokerGame(betManager.Object);
        }

        IPokerGame game;
        List<Mock<Player>> players;
        Mock<IBetManager> betManager;

        public EarlyGame()
        {
            // Create the necessary objects
            game = getGame();
            game.SmallBlind = SMALL_BLIND;
            game.BigBlind = BIG_BLIND;
            players = new List<Mock<Player>>(PLAYER_COUNT);
            
            // Configure players
            for (int i = 0; i < PLAYER_COUNT; i++)
            {
                var player = new Mock<Player>();
                player.CallBase = true;
                player.Object.Name = String.Format("Player {0}", i);
                player.Object.Balance = PLAYER_BALANCE;
                players.Add(player);
                game.AddPlayer(player.Object);
            }
        }

        [Fact]
        public void EarlyGame_BlindsCorrectlyApplied()
        {
            // Act
            game.NewGame();
            
            // Assert
            Assert.Equal(players[0].Object, game.SmallBlindPlayer);
            Assert.Equal(players[1].Object, game.BigBlindPlayer);
            betManager.Verify(x => x.PlaceBet(players[0].Object, SMALL_BLIND), Times.Once);
            betManager.Verify(x => x.PlaceBet(players[1].Object, BIG_BLIND), Times.Once);
        }

        [Fact]
        public void EarlyGame_InitialPlayerCorrect()
        {
            // Arrange
            IPlayer actual = null;
            game.NextPlayer += (o, e) => actual = e.Player;

            // Act
            game.NewGame();

            // Assert
            Assert.Equal(players[2].Object, game.CurrentPlayer);
            Assert.Equal(players[2].Object, actual);
        }

        [Fact(Skip = "To be refactored")]
        public void EarlyGame_CurrentBetCorrect()
        {
            // Act
            game.NewGame();

            // Assert CurrentBet is BIG_BLIND
            Assert.Equal(BIG_BLIND, game.CurrentBet);

            // Act, raise
            game.PlaceBet(BIG_BLIND + 150);

            // Assert
            Assert.Equal(BIG_BLIND + 150, game.CurrentBet);
        }

        [Fact]
        public void EarlyGame_PlaceBetRedirected()
        {
            // Arrange
            game.NewGame();

            // Act
            game.PlaceBet(300);

            // Assert
            betManager.Verify(x => x.PlaceBet(players[2].Object, 300), Times.Once);
        }

        [Fact]
        public void EarlyGame_StageAdvancesToFlop()
        {
            // Arrange
            game.NewGame();
            PokerGameEventArgs actual = null;
            game.NextBettingRound += (s, e) => actual = e;
            betManager.Setup(x => x.BettingRoundOver()).Returns(true);
            const int INSIGNIFICANT = 3;

            // Act
            foreach (var insignificant in players)
                game.PlaceBet(INSIGNIFICANT);

            // Assert
            Assert.Equal(PokerGameStage.Flop, game.Stage);
            Assert.Equal(PokerGameStage.Flop, actual.Stage);
        }

        [Fact]
        public void NewGame_BlindPlayersCorrectlySelected()
        {
            // Arrange
            players[1].Object.Balance = 0;

            // Act
            game.NewGame();

            // Assert
            Assert.Equal(players[0].Object, game.SmallBlindPlayer);
            Assert.Equal(players[2].Object, game.BigBlindPlayer);
        }

        [Fact]
        public void NewGame_ThrowsWithSinglePlayer()
        {
            // Arrange
            game.RemovePlayer(players[1].Object);
            game.RemovePlayer(players[2].Object);

            // Act & Assert
            Assert.Throws<PokerException>(delegate { game.NewGame(); });
        }

        [Fact]
        public void NewGame_ThrowsWhenGameActive()
        {
            // Arrange
            game.NewGame();

            // Assert
            Assert.Throws<PokerStageException>(delegate { game.NewGame(); });
        }
    }
}
