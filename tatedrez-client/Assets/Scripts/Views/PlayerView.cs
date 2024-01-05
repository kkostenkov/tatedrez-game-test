using System.Threading.Tasks;
using Tatedrez.ModelServices;
using Tatedrez.Views;
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private SquareView[] squares;
    [SerializeField]
    private TMP_Text playerName;

    public async Task Initialize(PlayerService playerService)
    {
        this.playerName.text = playerService.GetName();
        var i = 0;
        foreach (var piece in playerService.Pieces()) {
            squares[i].AssignPiece(piece);
            i++;
        }
    }
}
