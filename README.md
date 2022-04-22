# FYPII-MemoryJog-BabyNeuron
### This repository contains an implementation for 2 games (2D and 3D) of the same concept. 
## Intro
The purpose behind this R&D project is to evaluate the difference in cognitive gain by playing cognition targeting 2D and 3D games. There are many widespread researches focusing on cognition enhancement using Serious 2D Games, there are some on 3D as well. Our research however, focuses on the gap between the gain of these two types. Therefore, we have implemented a cognition boosting concept using gamification elements into a serious game with 2D and 3D versions 
## Memory Jog 
#### It contains a starting menu for registration and login for the children to be experimented on.
#### The screen after login shows the option to enter a Corsi Block Tapping test (see [Psy Tool Kit](https://www.psytoolkit.org/experiment-library/experiment_corsi.html)) if not already entered for a specific user (mostly first timers). It then takes the user to a main menu for the game which contains a desert like scenery. (assets picked from the Unity Assets Store).
### Main Menu
The main menu has 6 options. They are Normal Game, Timed Game, Scoreboard, Quit, 2D and 3D. From the Main Menu the user, firstly, selects 2D game or 3D game. Then, the user selects from Normal mode or Timed game. Both the normal mode and Timed Game button takes us to game difficulty screen. The Quit button undoes our 2D or 3D game selection.
### Difficulty
The difficulty screen consists of four options. They are Free Mode, Easy, Medium and Hard buttons.
####
Once a user selects an option from the difficulty screen, they are delivered to the Game Screen
### Game Screen
If you have selected the 2D option and Normal Game initially from Main Menu, then a 2D view of 9 x 9 grid will be shown and lives will be given according to the option selected in the Difficulty Screen. A path consisting of single tile will be highlighted after a 3 seconds timer. After the tile is highlighted the user will step on the highlighted tile. If correct tile is stepped on, then two tiles will be highlighted and so on. If wrong tile is stepped on, then a path with decreased length will be highlighted the next time and one life will be lost.
If you have selected the 3D option and Normal Game initially from Main Menu, then a 3D view of 9 x 9 grid will be shown and lives will be given according to the option selected in the Difficulty Screen. A third person view of the user’s avatar will be visible. A path consisting of single tile will be highlighted after a 3 seconds timer. After the tile is highlighted the user will step on the highlighted tile. If correct tile is stepped on, then two tiles will be highlighted and so on. If wrong tile is stepped on, then a path with decreased length will be highlighted the next time and one life will be lost.
If timed game option is selected with 2d or 3d option then fixed time will be given after path is highlighted every time.

## Authors
The authors of this R&D are all students of FAST NUCES Islamabad.
- Momin Salar 18i-0574
- Shahnoor Haider 18i-0460
- Umair Anwar 18i-0500
## References
- https://www.unity.com/
- https://www.udemy.com/course/unity-game-tutorial-3d-memory-game-3d-matching-game/
- Create a grid in Unity - Perfect for tactics or turn-based games! - YouTube
