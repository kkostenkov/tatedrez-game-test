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

        private void Awake()
        {
            clickDetector.Clicked += OnSquareClicked;
        }

        private void OnDestroy()
        {
            clickDetector.Clicked -= OnSquareClicked;
        }

        public void SetCoordsText(string text)
        {
            this.label.text = text;
        }

        public void AssignPiece(Piece piece)
        {
            Piece = piece;
            SetCoordsText($"{piece.Owner} {piece.PieceType}");
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