using UnityEngine;

[CreateAssetMenu(fileName = "PiecesSkin", menuName = "ScriptableObjects/SpawnPiecesSkinScriptableObject", order = 1)]
public class PiecesSkinScriptableObject : ScriptableObject
{
    [Header("White")]
    [SerializeField]
    private Sprite WhiteRook;
    [SerializeField]
    private Sprite WhiteKnight;
    [SerializeField]
    private Sprite WhiteBishop;
    
    [Header("Black")]
    [SerializeField]
    private Sprite BlackRook;
    [SerializeField]
    private Sprite BlackKnight;
    [SerializeField]
    private Sprite BlackBishop;

    public Sprite GetSprite(string pieceName, int side)
    {
        switch (pieceName) {
            case "Bishop":
                return side == 1 ? this.BlackBishop : this.WhiteBishop;
            case "Knight":
                return side == 1 ? this.BlackKnight : this.WhiteKnight;
            case "Rook":
                return side == 1 ? this.BlackRook : this.WhiteRook;
            default:
                return null;
        }
    }
}
