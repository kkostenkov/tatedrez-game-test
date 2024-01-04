using NSubstitute;
using NUnit.Framework;
using Tatedrez;

public class GameplaySessionControllerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameplaySessionControllerTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [Test]
    public void ShouldIncrementTurnNumberWhenPlayerMoves()
    {
        //
        var view = Substitute.For<IBoardView>();
    }
}
