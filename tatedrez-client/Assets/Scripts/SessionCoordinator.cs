using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
   public class SessionCoordinator : MonoBehaviour
   {
      [SerializeField]
      private GameSessionView sessionView;

      private GameSessionRepository sessionRepo = new GameSessionRepository();
      private GameSessionFlow flow;

      private async void Awake()
      {
         flow = new GameSessionFlow();
         var data = this.sessionRepo.Load();
         var inputManager = new PlayerInputManager();
         this.sessionView.BindLocalInputForPlayer(0, inputManager);
         this.sessionView.BindLocalInputForPlayer(1, inputManager);
         
         await flow.Prepare(data, this.sessionView, inputManager, inputManager);
      }

      private async void Start()
      {
         while (flow.IsRunning) {
            await this.flow.ProcessTurn();
         }
         await this.flow.ProcessTurn();
      }
   }
}