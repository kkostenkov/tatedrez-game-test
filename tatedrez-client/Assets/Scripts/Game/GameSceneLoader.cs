using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(1);
    }
}
