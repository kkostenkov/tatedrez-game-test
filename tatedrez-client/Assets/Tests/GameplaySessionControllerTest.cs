using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;

public class GameplaySessionControllerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameplaySessionControllerTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [Test]
    public async Task ShouldIncrementTurnNumberWhenPlayerPutsPiece()
    {
        var sessionData = new GameSession {
            CurrentPlayerTurnIndex = 0
        };
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var controller = new GameSessionController(sessionData, view, input);

        await controller.PlacePieceByPlayer(0);
        
        Assert.AreEqual(1, sessionData.CurrentPlayerTurnIndex);
    }

    }
}
