
Be sure to grab [lyn](http://feuniverse.us/t/ea-asm-tool-lyn-elf2ea-if-you-will/2986)


Editing the modular minimug box (henceforth just the MMB) involves two steps for users: Picking which modules to use and then setting the options for those modules. Both of these are done in one file, `MMB Configuration.txt`.

Modules are broadly grouped into types, such as modules that display minimugs, names, numbers, etc.

```

// Unit name modules

	//#define MMBName
	//#define MMBNameCentered
	#define MMBNameAffinityShifted

// Unit name options

	// Coordinates and widths are in 8x8 pixel tiles
	#define MMBNameWidth 7
	#define MMBNameColor TextBlack
	#define MMBNameX 5
	#define MMBNameY 3

```
*Shown: A snippet of `MMB Configuration.txt`.*

To disable a module, simply add `//` before it. In this example, we see that two out of the three available modules are disabled. For most types of modules it only makes sense for one module to be enabled.


After each list of modules is a set of options that affect the modules.

The most basic options affect the position of the module. Sometimes these are measured in tiles, usually for background elements, and other times they are measured in pixels. 

Some options specify a tile index to use. Most of the time these don't need to be edited, and users should be careful not to overwrite anything else.

Some modules may require the FE8 skill system to be included in the same buildfile.

Here is a list of current modules and descriptions of them:

MMBStandardTilemap
	This is the standard tilemap drawing routine. It draws a blue, green, or red palette depending on the allegiance of the unit. This tilemap is made up of:

A two byte header, containing width-1 and height-1
Rows, with the last row first, consisting of tile indexe shorts

An image called `Tile Indexes.png` shows the available (vanilla) tiles available for use with the tilemap. Combine the row number and column number to gain the tile index of the tile (i.e. the last tile in the image is $7F). Combine tile indexes with $400 to horizontally flip them, $800 to vertically flip them.

When changing the size of the tilemap, be sure to edit both `MMBWidth` and `MMBHeight`.

MMBName
	Shows the unit's name, aligned to the left.

MMBNameCentered
	Shows the unit's name, aligned to the center of allocated space

MMBNameAffinityShifted
	Shows the unit's name, aligned to the left. This module moves the name to the right 2 tiles if affinity is drawn. It's intended to be used alongside MMBAffinity, with the affinity icon drawn where the name moves from.

MMBMinimug
	Shows the unit's minimug the way vanilla does. This includes getting generic minimugs as normal and also the 'increase portrait by 1' bit.

MMBMinimugEnemyFlip
	Same as above but enemy minimugs face right.

MMBInventory
	Shows all items in a unit's inventory in a horizontal line.

MMBEquippedWeapon
	Shows only the unit's equipped item, if they have one.

MMBEquippedWeaponName
	Shows only the unit's equipped item, if they have one, along with the item's name. The coordinates for this should be evenly divisable by 8.

MMBInventoryOrEquippedWeaponName
	This shows the unit's full inventory if they are an enemy or their equipped weapon and weapon name if they are not. The coordinates for this should be evenly divisable by 8.

MMBHPStatus
	This draws an HP label, current HP, and max HP the way vanilla does, alternating with status every 64 frames.

MMBDEFRES
	This alternates between drawing a defense label + number and a resistance label + number every 64 frames

MMBDEFRES
	This alternates between drawing an avoid label + number and a dodge (crit avoid) label + number every 64 frames

MMBHPBar
	This draws a bar that represents current HP/max HP.

MMBAffinity
	This draws the unit's affinity, if they have one.

MMBSkillsAlternate
	This cycles through a unit's skills, if they have any. It switches to the next skill ever 64 frames. This module requires the skill system.

MMB___Number
	These modules draw their associated stats. Unusual numbers include DOD (dodge or crit avoid), CHR (skill system skill charge), and RTG (rating, a sum of a unit's stats).

MMB___Label
	These modules draw image labels for their associated stats. 




