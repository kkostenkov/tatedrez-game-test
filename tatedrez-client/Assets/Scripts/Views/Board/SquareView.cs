using TMPro;
using UnityEngine;


namespace Tatedrez.Views
{
    internal class SquareView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;

        public void SetCoordsText(string text)
        {
            this.label.text = text;
        }
    }
}