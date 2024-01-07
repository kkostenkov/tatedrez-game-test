Tatedrez by K.Kostenkov

The playable project consists of three modules. Each of them is wrapped in its own assembly definition.
- Tatedrez.Core - module with the business logic of the game. Standalone, most of it covered by unit tests.
- Tatedrez.Game - Unity-dependent module that defines view-independent classes and interfaces to implement. Depends on Core.
- Tatedrez.Views - All view-related stuff and some code to get started with Core and Game.

Supporting modules are
- Tatedrez.Editor - Editor only module. Includes a custom inspector for SquareView to be used in the *TweensSetup* scene.
- Tatedrez.Tests - Unit tests for the core. Has about 65% code coverage. Tends not to cover everything blindly, but the valuable business logic and avoids the fragile test problem. However, writing tests for "jump-over" moves could have saved me a few hours of debugging.  

The entry point is *SessionCoordinator.Start()*.  
The *DiBootstrap* class is used to install dependencies and manage the lifetime of the DI container object.  
*GameSessionController.Turn()* is a kind of state machine for the application logic that is called every time the turn is successful or not.  

The current full state of the game is in the *GameSessionData* class and could be serialised to store locally or send over the web.  
The local repository is *GameSessionRepository.cs*, which could be split into a game initialiser based on some game configuration and a game state loader.  
All model access is done through *ModelServices* classes to keep the model clean of logic.  

Providing a different implementation of *IInputSourceCollector* to *GameSessionView.BindLocalInputForPlayer* could allow implementation of AI or remote multiplayer modes. 

The *PiecesSkin* scriptable object could be loaded via addressables and injected to consumers for game reskinability.

Everything is built with an async approach in mind, so animations, asset loading or input handling operations may or may not block depending on the context.  

Separating the Core from the rest of the game helps with test writing and adherence to SOLID principles. With all due respect to them, they should be used to some extent, and although they are sometimes ignored in my project to keep unnecessary complexity (KISS) at bay, they could be easily implemented.  