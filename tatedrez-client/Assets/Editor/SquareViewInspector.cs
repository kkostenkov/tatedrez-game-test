using System.Threading.Tasks;
using UnityEditor;
using Tatedrez.Views;
using UnityEngine;

[CustomEditor(typeof(SquareView))]
public class SquareViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrawFoldout();
    }
    
    bool isFoldedOut = true;
    
    public void DrawFoldout()
    {
        this.isFoldedOut = EditorGUILayout.Foldout(this.isFoldedOut, "Tweens");
        if (this.isFoldedOut) {
            if (Selection.activeTransform) {
                DrawTweenButtons();
            }
        }

        if (!Selection.activeTransform) {
            this.isFoldedOut = false;
        }
    }

    private async Task DrawTweenButtons()
    {
        SquareView targetSquare = (SquareView)target;
        if (GUILayout.Button("Flash Red")) {
            await targetSquare.FlashRedAsync();
        }
    }
}
