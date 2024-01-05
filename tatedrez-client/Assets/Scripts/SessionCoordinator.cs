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

      private GameSessionRepository sessionRepo;

      private async void Awake()
      {
         var flow = new GameSessionFlow();
         var data = this.sessionRepo.Load();
         var inputManager = new PlayerInputManager();
         inputManager.AddInputSource(this.localInputManager, 0);
         inputManager.AddInputSource(this.localInputManager, 1);
         
         await flow.Prepare(data, this.sessionView, inputManager, inputManager);
      }
   }
}