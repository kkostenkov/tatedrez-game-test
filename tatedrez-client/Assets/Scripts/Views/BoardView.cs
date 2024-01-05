using System.Threading.Tasks;
using Tatedrez;
using Tatedrez.Models;
using UnityEngine;

public class BoardView : MonoBehaviour, IBoardView
{
    public async Task Build(GameSessionData sessionData)
    {
        var board = sessionData;
        // board size
        // pieces

        // for each player
        // not placed pieces

        // game stage
        return;
    }

    public Task ShowGameOverScreen()
    {
        throw new System.NotImplementedException();
    }

    public Task VisualizeMove(PlacementMove move)
    {
        throw new System.NotImplementedException();
    }

    public Task VisualizeMove(MovementMove move)
    {
        throw new System.NotImplementedException();
    }

    public Task ShowTurn(int playerIndex)
    {
        throw new System.NotImplementedException();
    }

    public Task VisualizeInvalidMove(PlacementMove move)
    {
        throw new System.NotImplementedException();
    }
}
