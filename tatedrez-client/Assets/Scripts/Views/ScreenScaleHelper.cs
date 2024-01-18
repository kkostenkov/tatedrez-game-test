using UnityEngine;

namespace Tatedrez.Views
{
    public class ScreenScaleHelper
    {
        public const float ReferenceScreenHeight = 600f;

        public static float ScreenScaleFactor => Screen.height / ReferenceScreenHeight;
    }
}