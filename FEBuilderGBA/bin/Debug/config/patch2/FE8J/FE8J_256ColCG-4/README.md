# FE8U_256ColBG
Vanilla FE8U allows backgrounds/CGs to use one of a few 16-colour palettes for each 8x8 tile. This patch provides functionality for supporting 256-colour mode BGs in FE8U. This allows any pixel in the background to use one of either 224 or 256 colours instead.

![Gif broken](https://i.imgur.com/LPLVsik.gif)

## Building
Apply [CGCol.event](CGCol.event) to a clean FE8U ROM using [Everything Assembler, AKA EA](https://feuniverse.us/t/event-assembler/1749) or run [MAKE HACK](MAKE HACK.cmd). Chapter 0's (Prologue: The Fall of Renais) events have been replaced by example events.

## Adapting to an existing project
If you're making a bigger ROMHack or project involving multiple patches and you're using [FEBuilderGBA](https://github.com/FEBuilderGBA/FEBuilderGBA/releases/) as your main tool, you might be familiar with [Custom Build](https://dw.ngmansion.xyz/doku.php?id=en:en:guide:febuildergba:skillsystems_custombuild). Whilst it's generally not recommended to mix buildfile patches and FEBuilderGBA, if the patch you're interested in using is not available in FEBuilderGBA, Custom Build is one way to still adapt the patch.

If you're using Custom Build or buildfiles as your tool/editor/w/e, keep in mind that this patch by default repoints the background table and sets new events for Chapter 0 (Prologue: The Fall of Renais). You will probably want to reorganize these.

## Implementation/Features
For 256-colour BGs, all background colours are reserved for the BG. This means regular chatbubbles won't work, as these also need background palettes. Loading portraits also breaks the BG. Instead, the CGTextBox (started by using event code [CGTEXTSTART](https://github.com/StanHash/EAStandardLibrary/blob/experimental/Language%20Raws/FE8/Scene-Text.txt#L83)) is now built solely out of sprites, allowing this chatbox to still be used when a 256-colour CG is displayed.

For 224-colour BGs, All but two background palettes have been reserved for the BG. The other two palettes are reserved for text and chatbubble. This allows displaying portraits and chatbubbles at the cost of fewer colours for the BG. I'll use the umbrella term "Multipalette BGs" to refer to both 224-colour BGs and 256-colour BGs.

Multipalette BGs are added to the usual conversation background table. 256-colour BGs have their TSA pointer set to 0, whereas 224-colour BGs have theirs set to 1. This is how the patch recognizes how to load the backgrounds, so make sure to keep this consistent for your BGs when you add them. I had to [repoint the background table](gfx/BG/BG.event), so if you plan on adding the patch to your buildfile project or [Custom Build](https://dw.ngmansion.xyz/doku.php?id=en:en:guide:febuildergba:skillsystems_custombuild), make sure it's not being repointed multiple times.

Adding your own Multipalette BGs means having to convert .png images to binary .dmp files. I made a program, [Sommie.exe](gfx/BG/Sommie.exe), to make this process simple. Here's how to add your own BGs:
- Make sure your image is in .png format and 256 pixels wide & 160 pixels tall. Keep in mind that the displayed image will be 240x160, meaning the last 16 columns of pixels won't display.
- Make sure your image is saved in indexed colour mode. If you're using [Usenti](https://www.coranac.com/projects/usenti/), just save the image as 8bpp.
- For 224-colour BGs, make sure none of the pixels in your 8bpp image use the last 32 colours in the palette or [Sommie](gfx/BG/Sommie.exe) will be sad and throw an error.
- Run [Sommie.exe](gfx/BG/Sommie.exe) (if you're using Windows) or [Sommie.py](gfx/BG/Sommie.py) in python3. The command would be `Sommie.exe <colCount> <pngfilename> <dmpfilename> <palfilename>` for the executable, or `python3 Sommie.py <colCount> <pngfilename> <dmpfilename> <palfilename>` for the python script.
  - `<colCount>` is either 224 or 256.
  - `<pngfilename>` is the filename of the input image.
  - `<dmpfilename>` is the filename of the output compressed image.
  - `<palfilename>` is the filename of the output palette.
  
  Easiest would be to open the batch-file [SommieAll.bat](gfx/BG/SommieAll.bat), which simply runs [Sommie.exe](gfx/BG/Sommie.exe) for all .png files in the directory. [SommieAll.bat](gfx/BG/SommieAll.bat) expects the first three characters of each `.png` file's name to be either `224` or `256`, which it'll use as input for `<colCount>`.
- `#include` the .dmp and the Pal.dmp files at the bottom of [BG.event](gfx/BG/BG.event) and add an entry to the background table. Make sure to set the TSA pointer to 0 or 1 for 256 or 224 colour BGs respectively.

There's two example images of [a certain guardian spirit](gfx/BG/256Sommie.png) which have been inserted already for reference. [One](gfx/BG/256Sommie.png) is a 256-colour BG, [the other](gfx/BG/224Sommie.png) a 224-colour BG. These can be seen in action in Chapter 0 (Prologue: The Fall of Renais). Feel free to remove the [example events](events/0.event) once you're familiar with everything/ready to add the patch to your project.

## Credits

circleseverywhere for [lzss.py](https://www.dropbox.com/sh/xe3bk2tn87zboif/AACTeniihbt-NQWrTpn6F5OSa?dl=0&preview=lzss.py)
7743 for finding bugs and suggesting improvements.