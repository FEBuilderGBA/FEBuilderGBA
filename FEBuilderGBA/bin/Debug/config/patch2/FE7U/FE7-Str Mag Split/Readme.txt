FE7 STR/MAG SPLIT README
BY TEQUILA

This hack, as you might have guessed, splits strength into two stats, strength and magic. Here are things that you, the user, need to (probably) change in order to make your hack use this stuff properly. 

-You'll need Event Assembler 10.0 or higher to install this.

- Bases, growths, and whatnot. There are 2 tables in the folder: Mag Char Table and Mag Class Table. There's nightmare modules for them. Should be easy to figure out. NOTE: If you change FreeSpace in the Master EA File, you'll have to update the locations in the .nmms as well. Open them in notepad, and change line 3 in Char Editor to your new FreeSpace entry and line 3 in Class Editor to FreeSpace+0x200.

- Definitions in the Master EA file. If you haven't repointed and expanded any tables, the only one that should really matter is the FreeSpace variable. You can put it wherever you'd like; just make sure you're not overwriting something important. If you want 'Mov' instead of 'Move' on the promotion and level-up screens (which I recommend, because it looks nicer), you'll have to do that yourself (via CC's text parser/FEditor/manual huffman encoding, whatever floats your boat) and then replace the defined text ID. Similarly for the r-button text of Talk option on the stat screen; currently it uses the affinity R-button text.

- Magic stat booster. This hack makes item 0x98, 5000 G, the magic stat booster. It's currently set to boost mag by 2, like other stat boosters. If you're already using that item ID for something else and want to expand the item table, you'll also have to repoint and expand the Item Usability and Item Effect tables (and the prep screen usability table, which is a teensy bit more complicated; see below) If you install Blazer's custom item patch (which, among other things, already (I believe) repoints and expands these tables), you'll have to remove the 0x4A from the definitions on lines 22 and 23.

- Prep screen usability table. I already had to expand this because item 0x98 isn't in the normal one. In Prep Screen and Mag Booster/Prep Screen Usability Table, take the item id of your new item, subtract 0x5A from it, multiply by 4, and then write 64 81 02 08 at that location in the table. To make sure the game actually sees this, go to line 164 in Prep Screen EA.txt and change BYTE 0x3E to BYTE (item id-0x5A)

- Stat boosters. Normally, the mag boost is stored at byte 0x9 of the stat boost struct (pointed to by the pointer at 0xC of the item struct) Since vanilla FE makes their stat boost structs 0xC bytes long, you shouldn't have to repoint any to Change stat boost of an item that already has one (like a legendary weapon) 

- Talk. Talk is a new option on the stat screen because people told me it didn't look nice to have a blank space there. It shows the first character in the CharacterTalkEvents section of your chapter events that this character can talk to. Once the event ID has been triggered, the next available character will be displayed (if there's none, it'll show --- like with Trv and Cond) It does NOT actually allow you to talk to anyone; it's just a feature from FE4.

- Weapon abilities. Since I kinda figured byte 0x40 of weapon ability 1 (do magic damage; currently only used by the light brand/runesword/wind sword) was redundant given that byte 0x2 (Magic) exists, I've repurposed it to be "hit opposite defensive type". This means that:
If 0x2 is not set and 0x40 is not set, you have a physical weapon that hits defense, as usual.
If 0x2 is set and 0x40 is not set, you have a magical weapon that hits res, also as usual.
If 0x2 is not set and 0x40 is set (magic swords), you have a physical weapon that hits res. This means it uses the wielder's strength.
If 0x2 is set and 0x40 is set (currently nothing), you have a magical weapon that hits def.
You shouldn't have to modify any existing weapons unless you want to make the magic swords use the wielder's mag instead of str, in which case you'd set byte 0x2 for those three items.

- Magic is stored at byte 0x39 of the character struct in ram, and the growth is stored at 0x7A of the battle struct. The latter was intended for a constitution growth before IntSys canned that idea, so it was unused until now, which means you can't have a con growth (or if you want to, you'll have to get a bit creative about it)

-My notes started out in the file called "str mag split notes.txt", but then they started ending up in the EA files where I needed them, so they're a bit scattered. Also, they probably don't make a huge amount of sense. I am so sick of this thing at this point that I'm not going to bother updating them. I will probably rue this when I have to bugfix, but too bad.

GLITCHES:
Yes. There are glitches. Sorry.
- The promotion screen had a flipping effect when the class changes on the bar at the top (the one that says Cavalier Lv 10 and changes to Paladin Lv 1) I couldn't figure out how to shift that effect upwards when I expanded the screen, so I had to disable it. It's almost certain that you never noticed it existed, but I'm mentioning it anyway, because I am an honest person.
- Similarly, the promotion/level-up screen with animations on has a bar of tiles that refuses to scroll in with the rest of the image. This means that when the stats zoom in from the left, there's going to be a row of tiles already there. It's a bit ugly, but again, I don't know how to fix it.
- Also similarly, the level-up screen with animations OFF (yes, they're different!) has a piece appear at the of the screen. This is because I made the image larger to accomodate 2 more stats, so it's juuuuuuust large enough to hang off the screen and (due to the way backgrounds work) loop in from the top. This will only be seen for the first and last 2 frames, so it really shouldn't be TOO noticeable, but it does irk me that I cannot fix it.
- I had to disable B/W/L because that's where I saved the mag stat when, y'know, saving. Otherwise, it'll show the wrong values.

CREDITS:
Brendor - for helping me save the stat and giving me his notes, even if I didn't understand them.
Circleseverywhere - emotional support and occasional giver of good ideas. Also, the BL macro.
Bookofholsety and Arch - for the Talk idea.
Lord Reyson - for helping me track down the combat calculations
Zane - fixing some OAM issues on the prep screen
FEU discord chat - for laughing at my increasingly glitchy images.
