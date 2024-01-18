using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Tatedrez.Views
{
    public class GameViewAnimator
    {
        public Task AnimateWinLine(UILineRenderer winLine, Vector2 begin, Vector2 end)
        {
            var durationSeconds = 1f;
            var seq = DOTween.Sequence();

            winLine.Points = new Vector2[2] {
                begin,
                begin
            };
            var drawingSeq = DOTween.To(() => winLine.Points[1], 
                x => {
                    winLine.Points[1] = x;
                    winLine.SetAllDirty();
                }, 
                end, 
                durationSeconds);
            
            seq.Append(drawingSeq);
            return seq.AsyncWaitForCompletion();
        }

        public static Task AnimatePieceMovement(Transform pieceGraphicsTransform, Vector3 origin, Vector3 destination,
            Transform tempParent)
        {
            var originParent = pieceGraphicsTransform.parent;
            var seq = DOTween.Sequence();
            seq.OnStart(() => {
                pieceGraphicsTransform.SetParent(tempParent);
            });
            var pickUpPiece = pieceGraphicsTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f);
            var movePiece = pieceGraphicsTransform.DOMove(destination, 0.2f);
            var putDownPiece = pieceGraphicsTransform.DOScale(Vector3.one, 0.1f);
            seq.Append(pickUpPiece).Append(movePiece).Append(putDownPiece);
            seq.OnComplete(
                () => {
                    pieceGraphicsTransform.SetParent(originParent);
                    pieceGraphicsTransform.position = origin;
                });
            return seq.AsyncWaitForCompletion();
        }
    }
}