This document describes how to modify the game.

In project view>_ChangableProperties there are 3 folders:

Actions> Change here how much experience is gained from that action. All 4 lists must have same amount of elements.
Buildings> Here you can change everything related with buildings:

If manual mode is turned on, it will take space by tiles compared to center. Automatic mode is based on largest sizes in game, so a tree will take up all space from its branches.
Pieces by level, x is level, y is amount allowed on that level. You can add more to this list, but x needs to increase every time.
If you play game in editor you need to reset memorized buildings in game before building the game.

Project view> prefabs has all the models and some of the smaller settings like how speed of animals and similar

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Scene view> Changable parameters, with several scripts, here you can change: 

experience needed for every level, last level will be 9999999 exp.
positioning of tools spawned when clicked on building
storage animation lenght
job board amounts and experiences
achievments, x is amount needed to get level y, so y should increase by 1 every time

Scene view> Managers has all the managing scripts

GameManagers has coin manager and diamond managers which set initial money and diamonds
UManager has visitor and villager buying parameters
SpawnManager has all the spawning intervals and speeds of for example villager, boat etc.
Tutorial has all the tutorial dialogs and where applicable has amounts needed to finish tutorial, like gather 3 eggs.

Scene view> roadsideshop has its own script with a setting how much items visitors buy from it.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

For adding new items to the game, it would best be to follow where an item appears. 
So would need a new building script, tool it interacts with, link to prefabs, prefabs need to have all the scripts other prefabs of the same type have,
memorized building (names must match). 
Would also need additions to UI where it appears, storage icons, shops, etc. Would also need proper tag that matches with name.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

General game flow from coding perspective is: click is done in InputManager, clickable game items implement IClick type interface which mostly call functions from singletons in Managers.
Tutorial flow is controlled by Tutorial Manager. Building making is done with building scripts.  



