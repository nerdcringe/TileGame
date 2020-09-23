# TileGame
Mini survival game test thingy.

## Controls
* WASD to move up, left, down, and right
* Press left mouse button to gather tiles
* Press right mouse button to place tiles
* Scroll mouse wheel to navigate inventory. Items in the inventory are sorted based on ID so order is always the same
* Press E to open crafting menu; use the same controls to gather/place tiles in the grid as you would in the world
* Press escape to pause

## Menus
### Title Menu
* Click "new" to generate new world
* Click "load" to generate previously saved world
* Click "settings" to change the volume
* Click "exit" to close the application
* If a world was previously generated in the current session, click "return to game" to resume playing the last generated world in case you haven't saved.

### Pause Menu
* Click "resume" to unpause
* Click on the text box "save name" to set the save file name for the next time the world is saved. If the text box is left empty, the name defaults to "save".
It will overwrite files of the same name, except in the case of the default name, where names will add a "\_1", "\_2", and so on.
* Click "save" to save the current game to a file.
* Click "save & leave" to save the current game and return to the title menu
* Click "leave" to return to the title menu without saving to a file. The game can be resumed by clicking "return to game" on the title menu screen

## Gameplay
* Gather magma and steel from inside caves
* Bots will start chasing you and break tiles in their way. Pour magma on their heads to melt them into steel, then collect the steel. Sometimes bots drop a circut board, which is a surprise tool that will help us later. They will damage your health if they come in contact with you
* Steel is inpenetrable to bots
* Remove the water from around a fish and wait to gather its meat. Cook the meat by putting it around a campfire. Walk into the cooked meat to regain your health, with a maxiumum of 4 health
* Acorns may drop from leaves; place water on acorn to grow a tree

### Crafting
* Wood flooring (allows you to walk over it but prevents bots from being summoned on it): 2 x 1 of log (either row)
* Campfire (cooks fish meat): Magma above log (either column)
* Door (only allows player entry through): 1 x 2 of steel (either column)
* Cannon (automatically defeats bots): 3 stone and 1 circut board (any configuration)
* Lamp (prevents bots from being summoned in a 7x7 tile area): magma above steel (either column)
