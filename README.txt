Bindings:
WASD- move
Space- jump
Shift- sprint
----------GAME----------
You can run the game by running "Horror game" application iside the
AiGame folder
----------Bindings---------
Ctrl- crouch
X-flashlight on/off
Right click- flashlight special mode
Tab-inventory open/close (don't press the close button insde the inventory, use tab;
you can also right click on picked-up items to use them)

Lighting:
Can be adjusted in unity editor if needed.

Dev tools:

o-Gain flashlight battery
p-Lose flashlight battery

h-hurt player, hide/unhide item in hand
j-heal player

i-toggle fusebox

1-Change enemy state to "not spawned"
2-Change enemy state to "patrol"
3-Change enemy state to "chase"
4-Change enemy state to "flee"
5-Change enemy state to "FirstHide"
6-Change enemy state to "goHeal"


Note:
The FistHide state requires the enemy to be spawned in and only works once.
This was intentional.

The goHeal only works if enemy is bellow 50% health.
This can be achieved by hurting the enemy with the special flashlight mode.

Tips:
Items can be picked up by left-clicking on them and doors can be opened by pressing e.
Light switches, closets, fuseboxes can be interacted with by pressing e as well.

You start the game in the house. Once you find a keycard, you will be able to use the elevator.
(Spoiler!! -> the keycard is in an office that has monitors)

The elevator is used by pressing e on the white box next to it.
IMPORTANT NOTE: I didn't develop it quite well so make sure to enter it when it opens.
There is no option to open the lift again as it automatically closes the doors and starts moving.
If you want to skip the first floor, you can just move the entire player GameObject in the Unity editor into
the asylum.

The asylum also has a few locked rooms that each need a key to unlock. This was done to showcase that the enemy
can open unlocked doors but will not open locked doors.

Remember: There is no official ending yet, this is merely a showcase of the A.I., so you can't kill the enemy yet...

