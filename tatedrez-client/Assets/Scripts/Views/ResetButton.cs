using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public void ResetUiButtonCallback()
    {
        SceneManager.LoadScene(0);
    }
}
