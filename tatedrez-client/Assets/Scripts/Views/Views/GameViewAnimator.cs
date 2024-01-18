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
    }
}