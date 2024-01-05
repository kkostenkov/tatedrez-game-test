using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
   public class SessionCoordinator : MonoBehaviour
   {
      [SerializeField]
      private GameSessionView sessionView;
      [SerializeField]
      private LocalInputManager localInputManager;

      private GameSessionRepository sessionRepo = new GameSessionRepository();
      private GameSessionFlow flow;

      private async void Awake()
      {
         flow = new GameSessionFlow();
         var data = this.sessionRepo.Load();
         var inputManager = new PlayerInputManager();
         inputManager.AddInputSource(this.localInputManager, 0);
         inputManager.AddInputSource(this.localInputManager, 1);
         
         await flow.Prepare(data, this.sessionView, inputManager, inputManager);
      }

      private async void Start()
      {
         while (flow.IsRunning) {
            await this.flow.ProcessTurn();
         }
      }
   }
}