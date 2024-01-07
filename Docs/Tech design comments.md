Tatedrez by K.Kostenkov

Playable project consitis of three modules. Each one encased in it's own assembly defitition.
- Tatedrez.Core - module with the business logic of the game. Self-sufficient, major part of it is covered by unit tests.
- Tatedrez.Game - Unity-dependent module that defines view-independent classes and interfaces to implement. Depends on Core.
- Tatedrez.Views - All view-related stuff and some code to kick it off with Core and Game. 

Support modules are:
- Tatedrez.Editor - Editor-only module. Contains custom inspector for SquareView to be used in TweensSetup scene.
- Tatedrez.Tests - Unit tests for Core. Has about 65% of code coverage. Tend to cover not everything blindly but the valuable business logic and avoid the Fragile Test problem. However writing tests for "jump-over" moves could have saved me a couple of hours of debugging.

Entry point is *SessionCoordinator.Start()*  
*DiBootstrap* class is used to install dependencies and manage the lifetime of DI container object.  
*GameSessionController.Turn()* is a kind of state machine for the app logic that gets called every time wether the move was successfull of not.

Current complete state of the game is located in *GameSessionData* class and could be serialized to store locally or send over the web.  
Local storage is *GameSessionRepository.cs* that could be split into game initializer based on some game config and game state loader.  
All access for models in in *ModelServices* classes to keep model clean of logic.  

Supplying a different implementation of *IInputSourceCollector* to *GameSessionView.BindLocalInputForPlayer* could allow implement AI or remote multiplayer modes.

*PiecesSkin* scriptable object could be loaded via addressables and injected to consumers for game reskinability.

Everything is built with async approach in mind so that animations, asset loading or input handling operations to new blocking or not unblocking depending on the context.

Separating Core from the rest of the game helps writing tests and keeping to SOLID principles. Whith with all the respect to them should be used to some extent and despite sometimes are ignored in my project to keep the unnnecesary complexity (KISS) at bay - could be introduced easy enough.