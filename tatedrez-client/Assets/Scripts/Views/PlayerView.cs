using System;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using TMPro;
using UnityEngine;

namespace Tatedrez.Views
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private SquareView[] squares;
        [SerializeField]
        private TMP_Text playerName;

        public Piece SelectedPiece { get; private set; }
        private SquareView selectedSquare;
        private bool IsSelectingSquare;

        private void Awake()
        {
            foreach (var square in this.squares) {
                square.SquareClicked += OnPocketSquareClicked;
            }
        }

        private void OnDestroy()
        {
            foreach (var square in this.squares) {
                square.SquareClicked -= OnPocketSquareClicked;
            }
        }

        public Task Initialize(PlayerService playerService)
        {
            this.playerName.text = playerService.GetName();
            var i = 0;
            foreach (var piece in playerService.Pieces()) {
                this.squares[i].AssignPiece(piece);
                i++;
            }

            return Task.CompletedTask;
        }

        public void EnablePieceSelection()
        {
            IsSelectingSquare = true;
        }

        public void DisablePieceSelection()
        {
            IsSelectingSquare = false;
            if (selectedSquare) {
                selectedSquare.SetHighlightActive(false);    
            }
            selectedSquare = null;
        }

        private void OnPocketSquareClicked(SquareView view)
        {
            if (!IsSelectingSquare) {
                return;
            }
            if (selectedSquare) {
                selectedSquare.SetHighlightActive(false);    
            }
            selectedSquare = view;
            if (selectedSquare.Piece != null) {
                selectedSquare.SetHighlightActive(true);
            }
        }
    }
}