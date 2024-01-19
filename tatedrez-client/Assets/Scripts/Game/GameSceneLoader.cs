using System;
using System.Threading.Tasks;
using Tatedrez.ModalViews;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : MonoBehaviour
{
    [SerializeField]
    private ModeSelectorView modeSelector;
    
    private async void Awake()
    {
        DI.Container.Register<IGameModeSelector, GameModeSelector>().AsSingleton();
        
        var delay = Application.isEditor ? 0 : 0.5f;
        await Task.Delay(TimeSpan.FromSeconds(delay));

        await ShowGameModeSelectionModal();
        
        SceneManager.LoadScene(1);
    }

    private Task ShowGameModeSelectionModal()
    {
        return this.modeSelector.GetChoice();
    }
}