using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public event Action Resetting;

    public void ResetUiButtonCallback()
    {
        Resetting?.Invoke();
        SceneManager.LoadScene(1);
    }
}
