using System.Text;
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
        public Task ShowPlayerToMakeMove(int playerIndex)
        {
            var sb = new StringBuilder();
            sb.Append(playerIndex == 0 ? "<-- " : string.Empty);
            sb.Append($"Player {playerIndex}");
            sb.Append(playerIndex == 1 ? " -->" : string.Empty);
            this.playerToMoveLabel.text = sb.ToString();
            return Task.CompletedTask;
        }

        public Task DisplayTurnNumber(int currentTurnNumber)
        {
            this.turnNumberLabel.text = $"Turn: {currentTurnNumber}";
            return Task.CompletedTask;
        }
    }
}