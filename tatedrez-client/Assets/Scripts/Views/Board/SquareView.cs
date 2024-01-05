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
            UpdateHelperText();
        }

        public void AssignPiece(Piece piece)
        {
            Piece = piece;
            UpdateHelperText();
        }

        private void UpdateHelperText()
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
    }
}