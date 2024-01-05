using System;
using Tatedrez.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tatedrez.Views
{
    internal class SquareView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        [SerializeField]
        private Image highlight;
        
        [SerializeField]
        private Image piecePicure;

        [SerializeField]
        private EmptyClickDetector clickDetector;

        public event Action<SquareView> SquareClicked;

        public Piece Piece { get; private set; }
        public BoardCoords Coords { get; private set; } = BoardCoords.Invalid;

        private void Awake()
        {
            clickDetector.Clicked += OnSquareClicked;
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
            }
            else {
                this.label.text = Coords.ToString();
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
    }
}