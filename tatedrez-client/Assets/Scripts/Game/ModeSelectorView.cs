using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Tatedrez.ModalViews
{
    public class ModeSelectorView : MonoBehaviour
    {
        [SerializeField]
        private Button singleplayerButton;
        
        [SerializeField]
        private Button multiplayerButton;

        private TaskCompletionSource<bool> completionSource;
        private IGameModeSelector selector;

        private void Start()
        {
            this.selector = DI.Container.Resolve<IGameModeSelector>();
            this.singleplayerButton.onClick.AddListener(OnSinglePlayerClicked);
            this.multiplayerButton.onClick.AddListener(OnMultiPlayerClicked);
        }

        private void OnDestroy()
        {
            this.singleplayerButton.onClick.RemoveAllListeners();
            this.multiplayerButton.onClick.RemoveAllListeners();
        }

        public Task GetChoice()
        {
            this.gameObject.SetActive(true);
            this.completionSource = new TaskCompletionSource<bool>();
            return this.completionSource.Task;
        }

        private void OnSinglePlayerClicked()
        {
            if (this.completionSource != null) {
                selector.SetSinglePlayerMode();
                this.completionSource.SetResult(true);
            }

            Close();
        }

        private void OnMultiPlayerClicked()
        {
            if (this.completionSource != null) {
                selector.SetMultiPlayerMode();
                this.completionSource.SetResult(true);
            }

            Close();
        }

        private void Close()
        {
            this.gameObject.SetActive(true);
        }
    }
}