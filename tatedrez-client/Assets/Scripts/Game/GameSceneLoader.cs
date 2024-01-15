using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    private async Task Start()
    {
        var delay = Application.isEditor ? 0 : 0.5f;
        await Task.Delay(TimeSpan.FromSeconds(delay));
        SceneManager.LoadScene(1);
    }
}
