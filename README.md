UnityUIExample
--------

The purpose of this example is to help you become familiar with Unity 4.6+ UI system.  It should work on all platforms and device sizes.

* Please read all sections of this guide.  
* Questions: [@jasonrwalters](http://twitter.com/jasonrwalters)


Helpful Resources
--------
* Unity Beginner UI Lessons: [http://unity3d.com/learn/tutorials/modules/beginner/ui](http://unity3d.com/learn/tutorials/modules/beginner/ui)
* Unity Manual - UI: [http://docs.unity3d.com/Manual/UISystem.html](http://docs.unity3d.com/Manual/UISystem.html)


Requirements
--------
* [Unity 4.6+](http://unity3d.com/unity/download)


Demo
--------
* Run `example` in the Scenes folder and press play in the Unity editor.


GameController.cs
------
The game controller is the game's core and handles base functions, organized in the following sections:

* Setup - used to setup game variables that runs in `Start()`
* Game States - controls the various states `{ MainMenu, GamePlay, GameOver, Settings, Credits }`
* Game Score - this example counts seconds for score.  good for an "Endless Runner".
* Audio - this example has one sound for all UI button presses.
* Input - this example uses touch and keyboard commands.

Notes:
* `pubic enum GameStates` defines the various game states and for readability.
* `public void` methods are tied to UI buttons.  Examine the button's inspector to see configuration.
* `scoreDecimals` determines how many decimal places to display in the score.  can be edited in the inspector.
* Game Objects are active/deactive based on game state.
