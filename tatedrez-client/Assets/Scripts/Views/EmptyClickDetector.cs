using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tatedrez.Views
{
    public class EmptyClickDetector : Graphic, IPointerClickHandler
    {
        public event Action Clicked; 
    
        public void OnPointerClick(PointerEventData eventData)
        {
            this.Clicked?.Invoke();
        }
    }
}