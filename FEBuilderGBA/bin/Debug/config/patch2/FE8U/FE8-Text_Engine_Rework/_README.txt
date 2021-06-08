FE8 Text Interpreter Rework README
By Tequila

This hack adds some new features to the existing text engine.
It IS backwards compatible with existing text.
It is NOT compatible with Zeta's AutoNewLine hack.
All arguments MUST be non-zero because 0 is interpreted as a terminator when copying text from the rom. (This is why portrait ids, which are shorts, have 0x100 added, incidentally, so that portrait 0x02 is written as 0x0102 or [0x02][0x01].)
THIS IS MEANT TO BE USED WITH STANDARD CUTSCENE TEXT. It has not been tested with world map text, tutorial-style text boxes, scroll box (the parchment looking stuff), brown text boxes (like the one that says Renais Castle in the into cutscene), or anything else. Some of the features may work, some may crash the game, and some may do absolutely nothing because the game uses a different text interpreter in that instance.

When you load a portrait using LoadFace, the game will assign default values for font, color group, background palette, box type, and pitch to that portrait's location (these are explained later). If you update these values, they will be remembered in the future. For example, if you load Eirika's portrait and have her use the bold font, all of her future speech bubbles will use the bold font until you either change the font again manually, clear the portrait, or end the text. If Eirika and Seth have a conversation where her speech is bold and his speech is normal, you will not need to manually change the font every time the speaker changes. If you move a portrait to a new position, these characteristics will be copied over to the new position.

Any tables referenced can be found in _Text_Engine_Tables.txt.




######### Fonts
Syntax: [0x80][0x26][XX]
XX is an index in FontGlyphsPointerTable (0x01-0xFF). By default, 1 is the normal FE8 font, 2 is bold, and 3 is italics.
How to create a custom font (in a messy manner that I don't entirely understand myself and requires python):
1: Create a .png of your font. Look at Fonts/FE8_Text_Norm.png for a reference if needed.
2: Edit Fonts/generate_font.py and run it to create your font's installer. (requires Python 3 and was made by Zahlman, so I don't exactly know how it works)
3: Include the resulting installer in the relevant place in _Text_Engine_Tables.txt.
NOTE: If using the included bold/italic fonts, you may wish to manually edit the kerning (spacing between letters) for each glyph, otherwise some of the letters are spaced too far apart. The first 8 bytes of each glyph is the header, and the 6th byte is the width. As an example, here is the entry for 'A' in the italic font:
ital_text65:
BYTE 0x00 0x00 0x00 0x00 0x00 0x09 0x00 0x00
The 6th byte is 0x09, which is the width in pixels. The next letter will be drawn 0x09 pixels to the right of this A. If you make the value too small, the A will be overlapped; too large, and there'll be unnecessary white space between letters. Making a nice-looking font is HARD.


######### Change Text Palette
Syntax: [0x80][0x27][XX][YY]
XX is the color group (0x01-0x05) and YY is the index in TextPaletteTable.
COLOR GROUP refers to which section of the palette is used. A palette is 16 colors, and text is 2bpp (2 bits per pixel), or 4 colors, with the first color always being transparent. If each color group uses the same transparent color, there is room for 15/3 = 5 color groups in your palette. The first group uses colors (1,2,3,4), the second uses (1,5,6,7), the third uses (1,8,9,10), etc.
In vanilla FE8, color group 1 is used for world map text, group 2 is used for default conversation text, and group 5 is used for the [ToggleRed] command. Groups 3 and 4 aren't used by default; they are blue and yellow, respectively, but don't contrast well.
When you use this command, you are updating the colors of a specific color group. Therefore, any text on-screen that uses that color group WILL update.


######### Change Color Group
Syntax: [0x80][0x28][XX]
XX is the color group (0x01-0x05) (see the Change Text Palette section for an explanation on color groups).
This command changes which color group will be used, so you can have text with different colors on-screen at the same time. [0x80][0x28][0x05] is functionally identical to [ToggleRed] when changing from the normal text color. This just allows you to use the other parts of the palette bank at the same time. Just be sure to use the Change Text Palette command first if you're using groups 3 or 4, because the default palette for those groups is pretty hard to read.


######### Change Box Background Palette
Syntax: [0x80][0x29][XX]
XX (0x01-0xFF) is an index in TextBoxBgPaletteTable.
This is pretty self-explanatory. It changes the palette of the text bubble.


######### Change Box Type
Syntax: [0x80][0x2A][XX]
XX (0x01-0x7F) is an index in TextBoxTypePointerTable. Bit 0x80 is set if you want tails on (the part of the bubble showing which character is talking).
Changes the text box graphics. Use this before opening a new text bubble.
Unfortunately, this sounds more exciting than it actually is, due to the fact that there's not that many unique tiles in the text box graphics. I had envisioned something similar to comic book-styled boxes, with big spiky boxes to represent anger and whatnot, but that would require a heck of a lot more work than I'd be willing to put in. That being said, there is a quasi-spiky box and a thought bubble box included. The default text box has several frames where it expands, while the new boxes do not.


######### Change Box Height (aka 1- or 3-line boxes)
Syntax: [0x80][0x2B][XX]
XX is the number of lines (0x01-0x03).
Pretty self-explanatory.


######### Change Text Boop Pitch
Syntax: [0x80][0x2C][XX]
XX goes from 0x01-0x19, with 0x0D being the default pitch. Each value added is one semitone higher, up to 0x19, which is one octave higher than 0xD. Similarly, 0x01 is an octave lower than 0x0D.


######### Play Sound
Syntax: [0x80][0x2D][0x8D][0x8C][0x8B][0x8A]
This plays sound 0xABCD. It's not very intuitive, alas, but it seemed to be the easiest way to encode a short with the possibility of having a 00 somewhere and accidentally terminating your text. Don't forget to put your id in little endian!


######### Change Portrait Location
Syntax: [0x80][0x2E][XX][YY]
XX (0x01-0x08) is the position that you want to change. 01 is FarLeft, 02 is MidLeft, 03 is Left, 04 is Right, 05 is MidRight, 06 is FarRight, 07 is FarFarLeft, and 08 is FarFarRight.
YY is the new SIGNED x coordinate of the position, in tiles. The default values are (in the same order as above) 0x03, 0x06, 0x09, 0x15, 0x18, 0x1B, 0xF8 (or -8), and 0x26.
NOTE: If you want to change the position to tile 00, set YY = 0x80.
Example: If you wanted to have 4 people on the right side, you could move Left (0x3) 3 tiles to the left of Right (0x4) by using [0x80][0x2E][0x03][0x12]


######### Fancy LoadFace
Syntax: [0x80][0x2F][Options|0x80][Font][ColorGroup][BgPalette][BoxType][Pitch]
Ok, this takes a lot of arguments, but don't panic! It's not as bad as you think.
[Options] is a bitfield:
	0x80 is always set, to avoid the possiblity of an accidental terminator (00) if you don't use any of the other options.
	0x01 is set if you want to flip the portrait so that it faces rightward. If NOT set, the portrait will ALWAYS face left, independent of which of the screen it appears.
	0x02 is set to load the portrait with eyes closed. This avoids requiring another portrait slot (what vanilla FE8 does) or loading the portrait and immediately using [CloseEyes], because that looks weird.
Font, ColorGroup, BgPalette, BoxType, and Pitch are exactly as described earlier. You don't need to use Fancy LoadFace unless you want the flipping/eyes closed; you can use a normal LoadFace and then change each attribute individually, if you'd like.
The default values are 0x01, 0x02, 0x01, 0x01, 0x0D; they are the values set when using the normal LoadFace.


######### Move Portrait with Variable Speed
Syntax: [0x80][0x3X][YY]
The X (0-7) is the position ID. 0=FarLeft, 1=MidLeft, 2=Left, 3=Right, 4=MidRight, 5=FarRight, 6=FarFarLeft, 7=FarFarRight.
YY is the number of frames it takes the portrait to move to its new location.
It works exactly the same as your default MoveMidLeft and whatnot, but you can choose how long it takes.


######### Set Text Speed
Syntax: [0x80][0x38][XX]
XX is the number of frames between characters. For reference, vanilla FE8 has slow=8, normal=4, fast=1, max=0 (table at D7F58). Use 0xFF to return to the default speed (whatever speed is selected under Options).



If you use buildfiles with TextProcess, you may want to add the following commands to your ParseDefinitions file (feel free to rename as you like):
If using FEBuilder, I believe you use @ instead of [], so [Font] would be @0080 @0026.

[Font] = [0x80][0x26]
[TextPalette] = [0x80][0x27]
[ColorGroup] = [0x80][0x28]
[BoxBgPalette] = [0x80][0x29]
[BoxType] = [0x80][0x2A]
[BoxHeight] = [0x80][0x2B]
[BoopPitch] = [0x80][0x2C]
[PlaySound] = [0x80][0x2D]
[MugLoc] = [0x80][0x2E]
[LoadFaceFancy] = [0x80][0x2F]
[MFL2] = [0x80][0x30]
[MML2] = [0x80][0x31]
[ML2] = [0x80][0x32]
[MR2] = [0x80][0x33]
[MMR2] = [0x80][0x34]
[MFR2] = [0x80][0x35]
[MFFL2] = [0x80][0x36]
[MFFR2] = [0x80][0x37]
[TextSpeed] = [0x80][0x38]
[DefaultAttrs] = [0x01][0x02][0x01][0x01][0x0D] 			(for use with LoadFaceFancy)


EXAMPLE TEXT (shown as a gif on FEU)

[TextSpeed][0x1]
[oml][LoadFace][0x14][0x1]
[omr][LoadFace][0x2][0x1]
[oml]
Hi! This is a demonstration[N]
of the new text engine hack![A]
[omr]
Featuring things like...[A]
[oml]
[Font][0x2]CUSTOM[N]
[Font][0x3]FONTS![A]
[omr]
[ColorGroup][0x1]The [ColorGroup][0x2]ability [ColorGroup][0x3]to [ColorGroup][0x4]use [ColorGroup][0x5]the[N]
[ColorGroup][0x1]entire [ColorGroup][0x2]text [ColorGroup][0x3]palette![A]
[oml]
[0x15]
[Font][0x1]
[BoxBgPalette][0x3]
The ability to change the[N]
text box background palette![A]
[omr]
[ColorGroup][0x2]
[BoxType][0x3][ToggleMouthMove]
Custom box shapes![ToggleMouthMove][A]
[oml]
[BoxBgPalette][0x1]
[BoxHeight][0x3]
[BoopPitch][0x4]
Different pitches for your text noises![N]
[BoopPitch][0x10]
Even if you can't hear it, because this[N]
is a gif, trust me, it's there.[A]
[BoopPitch][0xD]
[omr]
[BoxType][0x1]
[BoxHeight][0x1]
Oh yeah, boxes can have three lines.[A][N]
Or one line, if that's more your style.[A]
[oml]
[BoxHeight][0x2]
The ability to play sounds, now with a[N]
sensible encoding scheme (sorry, Zahl!)[A]
[or]
[MugLoc][0x4][0xD]
[LoadFaceFancy][0x4][0x1][0x2][0x1][0x2][0x1][0x1][0xD]
[omr]
A fancier LoadFace that allows you to[N]
flip mugs as needed...or not![AN]
Also, you can change the on-screen[N]
coordinates of the different positions.[AN]
[or][MFFL2][0x60]
[offl][MFFR2][0xC]
[omr]
And as you just saw, you can adjust[N]
the speed at which they move, too.[A]
[oml]
Finally, you can also[N]
[TextSpeed][0x8]adjust the text speed.[TextSpeed][0x1][A]
[omr]
Well, that's all! This will be made[N]
available to the public after FEE3.[AN]
It will, of course, be free to use,[N]
and you should feel free to donate,[AN]
because if you don't, then the[N]
offending material will be removed.[AN]
You have been warned.[A][X]



CREDITS (that aren't Tequila):
Zahlman - made the original text engine overhaul for fe7, and wrote the python program to turn a png into something insertable
Stan - troubleshooting
Black Mage and Eliwan - graphics for the new text boxes
The Awful Emblem team - beta testing