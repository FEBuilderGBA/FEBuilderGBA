https://github.com/ZaneAvernathy/ModularMinimugBox/

# Installation

To install the Modular Minimug Box (henceforth just the MMB) you simply need to `#include` one of the `MMBInstaller` files in the `Installers` folder. There are multiple variants of the installer included with the MMB, so pick your favorite to install or use as a starting point to customize.

The MMB takes up a considerable amount of freespace and is made by default to be included within a larger EA buildfile. If you're installing the MMB on its own, you'll need to modify your installer to assemble the hack to freespace. The easiest way to do that is to add the line `ORG 0x00B2A610` as the first line in the installer, replacing `0x00B2A610` with the offset of some available space.

# Customization

To edit items on the MMB, there are two steps: Enabling/disabling modules and configuring module options.

Throughout the installer there are lists of modules

```

// Unit name modules

	//#define MMBName
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor.
		 */
	//#define MMBNameCentered
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor, centering it within the
		 * number of tiles specified by MMBNameWidth.
		 */

	#define MMBNameAffinityShifted
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor. If the unit has an affinity,
		 * the name is shifted 2 tiles to the right.
		 */
```

and their options

```

// Unit name options

	// Coordinates and widths are in 8x8 pixel tiles
	#define MMBNameWidth 7
	#define MMBNameColor TextBlack
	#define MMBNameX 5
	#define MMBNameY 3

```

To disable a module, comment it out (type `//` before a line's text) from the list of modules.

For example, if I didn't want the unit's name to be shown on the MMB my unit name modules would look like:

```

// Unit name modules

	//#define MMBName
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor.
		 */
	//#define MMBNameCentered
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor, centering it within the
		 * number of tiles specified by MMBNameWidth.
		 */

	//#define MMBNameAffinityShifted
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor. If the unit has an affinity,
		 * the name is shifted 2 tiles to the right.
		 */
```

Conversely, you select which module(s) to use by uncommenting them.

For example, if I just wanted the unit's name with no centering:

```

// Unit name modules

	#define MMBName
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor.
		 */
	//#define MMBNameCentered
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor, centering it within the
		 * number of tiles specified by MMBNameWidth.
		 */

	//#define MMBNameAffinityShifted
		/*
		 * Draws the unit's name to [MMBNameX, MMBNameY]
		 * using MMBNameColor. If the unit has an affinity,
		 * the name is shifted 2 tiles to the right.
		 */
```

To make implementation simpler, certain module types only support one module from their list being used at a time, such as the unit name modules.

After deciding which module(s) you want from a list, configuring them is as simple as editing numbers in their options.

Multiple modules with similar purposes share options. For example, all unit name modules share the same options.

Many module options have comments about how the options are interpreted, such as

`// Coordinates and widths are in 8x8 pixel tiles`

Many modules need to be positioned by tiles while some are positioned by pixels. It might be confusing to have both, but it would be far worse to have either all positions be in pixels (which would cause modules that must be tile aligned to have their coordinates silently floored to the nearest tile) or be in tiles (severely limiting things like icons and numbers).

Some module options have advanced options like which tile index to write to/use in VRAM. By default, none of these need to be edited and are included for completeness.

Modules involving skills require the FE8 skill system to be included within the same buildfile.

# Modules

Please see `Installers/MMBInstallerDefault.event` for descriptions of each module.

# Creating New Modules (Advanced users)

The MMB includes two utility files (`build.bat` and `Makefile` in the root folder) that can be used when developing modules. I can't guarantee that these will work for your environment out of the box. Both expect that you have [devkitARM](https://devkitpro.org/wiki/Getting_Started), [lyn](http://feuniverse.us/t/ea-asm-tool-lyn-elf2ea-if-you-will/2986), and [EA](https://feuniverse.us/t/event-assembler/1749). The Makefile assumes that you have [Stan's ea-dep](https://github.com/StanHash/ea-dep).

Creating a MMB module requires some knowledge about THUMB, EA, and lyn. Taking a look at existing modules might be helpful, and you're free to contact me on FEU or the FEU Discord server.

There are three parts of a module:

* The code
* The inclusion and setup
* The array entry

MMB modules always have two inputs:

* r0: A pointer to the MMB proc state
* r1: A pointer to the selected unit's data in RAM (Most of the time)

The coordinates for placing BG elements on the MMB can be found by `(2 * ((32 * YCoord) + XCoord)) + TargetBuffer` where `TargetBuffer` is either `WindowBuffer` or `WindowBufferBG1` (both definitions can be found in `CommonDefinitions.inc` which I recommend you include in your asm)

For drawing sprites to the MMB, it's important to get the vertical offset of the box in order to add it to the sprite's Y coordinate. Here's an example from the level number drawing module:

```

	@ Get positions for numbers

	ldr		r6, MMBLevelX
	ldr		r7, MMBLevelY

	@ check for lower window

	mov		r0, r4 @ MMB Proc state

	@ First we get the window's position

	add		r0, #WindowPosType
	ldrb	r0, [r0]
	lsl		r0, r0, #0x03
	ldr		r1, =WindowSideTable
	add		r0, r1, r0

	@ Then we grab which half of the screen it's on

	mov		r1, #0x03
	ldsb	r0, [r0, r1] @ -1 for top, 1 for bottom
	cmp		r0, #0x00
	blt		SkipBottom

	@ If it's on the bottom of the screen we subtract the MMB's height
	@ from the vertical size of the screen (20 tiles), multiply by
	@ the size of a tile, and then add that to our Y coordinate

	ldr		r0, MMBHeight
	mov		r1, #20
	sub		r1, r1, r0

	lsl		r1, r1, #0x03
	add		r7, r7, r1

SkipBottom:

```

Step two is to turn your ASM into a `.lyn.event` file. There are two helpful files included in the root folder (`build.bat` and `Makefile`) that can be used to convert your sources, depending on your environment. Previously, the MMB would use `#inctevent lyn "Some/File.elf"` within `MMBCore.event` but I have since switched to pregenerating the `.lyn.event` files for assembly speed.

In `MMBCore.event` there is a section that looks like:

```

	// Module inclusions

	// Tilemap modules

		#ifdef MMBStandardTilemap
			#include "Modules/MMBDrawTilemap.lyn.event"
			POIN MMBTilemap
			WORD MMBTilemapPaletteIndex
		#endif // MMBStandardTilemap

```

There are a few parts to this, so let's break them down.

First, `#ifdef MMBStandardTilemap`. `MMBStandardTilemap` is going to be the name of the module in the installer. The installer defines `MMBStandardTilemap` when the module is used, otherwise it isn't defined. Using this ifdef system we can keep the size of the MMB smaller by not assembling modules that are not used.

Second, `#include "Modules/MMBDrawTilemap.lyn.event"`. This uses `lyn` to include the contents of our elf file like it was a dmp, but it allows us to use outside symbols, like EA labels, definitions, and things from other elfs (like `Internal/Definitions.elf`).

Third, we have some EA literals that correspond to options in our installer. I suggest doing math on these here rather than in your assembly in order to save time and space.

Afterwards, be sure to add your module's info to the installer.

The third and final step for adding a module is to add an entry to one of two arrays:

* Build Routines
* Dynamic Routines

Build routines are run once when the MMB is being built. Dynamic routines are run every proc tick. BG elements are usually build routines, but some, like status drawing, are dynamic. Dynamic routines can have a dramatic effect on lag, so it's best to avoid heavy calculations or drawing. For example, modules that draw a unit's inventory will copy the icons to VRAM as a build routine but display the sprites using a dynamic routine, to avoid rewriting the icons every tick.

To pick an array, locate them (they're at the end of `MMBCore.event`) and add something along the lines of

```

	#ifdef MMBIconPrep
		POIN MMBPrepIcons
	#endif // MMBIconPrep

```

Where, once again, the `#ifdef MMBIconPrep` is the name of the module. The `POIN MMBPrepIcons` is a pointer to the routine.

