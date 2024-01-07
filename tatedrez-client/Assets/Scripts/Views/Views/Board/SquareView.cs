using System;
using System.Threading.Tasks;
using DG.Tweening;
using Tatedrez.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tatedrez.Views
{
    public class SquareView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        [SerializeField]
        private Image highlight;
        
        [SerializeField]
        private Image piecePicure;

        [SerializeField]
        private PiecesSkinScriptableObject defaultSkin;

        [SerializeField]
        private EmptyClickDetector clickDetector;

        public event Action<SquareView> SquareClicked;

        public Piece Piece { get; private set; }
        public BoardCoords Coords { get; private set; } = BoardCoords.Invalid;

        private void Awake()
        {
            clickDetector.Clicked += OnSquareClicked;
            this.label.enabled = false;
        }

        private void OnDestroy()
        {
            clickDetector.Clicked -= OnSquareClicked;
        }

        public void AssignCoords(BoardCoords coords)
        {
            this.Coords = coords;
            UpdateGraphics();
        }

        public void AssignPiece(Piece piece)
        {
            Piece = piece;
            UpdateGraphics();
        }

        private void UpdateGraphics()
        {
            if (Piece != null) {
                this.label.text = $"{Piece.Owner} {Piece.PieceType}";
                this.piecePicure.sprite = this.defaultSkin.GetSprite(Piece.PieceType, Piece.Owner);
                this.piecePicure.enabled = true;
            }
            else {
                this.label.text = Coords.ToString();
                this.piecePicure.sprite = null;
                this.piecePicure.enabled = false;
            }
        }

        protected void OnSquareClicked()
        {
            this.SquareClicked?.Invoke(this);
        }

        public void SetHighlightActive(bool isActive)
        {
            highlight.enabled = isActive;
        }

        public Transform GetPieceGraphicsTransform()
        {
            return this.piecePicure.transform;
        }

        public Task FlashRedAsync()
        {
            var originalColor = highlight.color;
            
            var seq = DOTween.Sequence();
            seq.OnStart(() => {
                highlight.enabled = true;
                highlight.color = Color.clear;
            });
            var transitionToRed = highlight.DOColor(Color.red, 0.1f);
            var restoreColor = highlight.DOColor(Color.clear, 0.2f);
            seq.Append(transitionToRed).Append(restoreColor);
            seq.OnComplete(
                () => {
                    highlight.enabled = false;
                    highlight.color = originalColor;
                });
            return seq.AsyncWaitForCompletion();
        }
    }
}