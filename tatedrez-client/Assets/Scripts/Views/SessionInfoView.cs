using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Tatedrez.Views
{
    internal class SessionInfoView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text playerToMoveLabel;
        [SerializeField]
        private TMP_Text turnNumberLabel;
        public async Task ShowPlayerToMakeMove(int playerIndex)
        {
            this.playerToMoveLabel.text = $"Player {playerIndex}";
        }

        public async Task DisplayTurnNumber(int currentTurnNumber)
        {
            this.turnNumberLabel.text = $"Turn: {currentTurnNumber}";
        }
    }
}