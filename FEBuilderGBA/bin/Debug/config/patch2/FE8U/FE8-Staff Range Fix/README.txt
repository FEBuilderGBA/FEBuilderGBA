FE7 Staff/Range Fix
By Tequila

This hack rewrites the way the game displays weapon/staff ranges; ie, the red/green squares that display how far away you can whack someone upside the head with the item of your choice. Originally, weapons that weren't in the set of designated ranges (1, 2, 1-2, 2-3, 3-10, 3-15) wouldn't display the range properly in the R-button examine and wouldn't highlight the proper amount of squares, but would still be able to attack at whatever range value was input. Staff ranges were hardcoded (except Torch), and even if you figured out where that value was, there were only 3 valid ranges that would display green squares: 1, 1-2, and 1-mag/2.

Now, staffs read their range value from the 0x19th byte of the item struct, just like weapons. Note that the range byte is broken into its nibbles to be read as {min, max}. So a 1-2 range weapon has range byte 0x12.
Some new functionality has been added:
- having a max nibble of 0 will use max{5, mag/2}. This was standard for status staves, for instance. Now, however, you can also apply this to weapons AND set the min to whatever you'd like, rather than having it be automatically set to 1. That being said, it'll still display Mag/2 on the item's R-button examine and the Equipment tab on the inventory screen, so if you have a minimum other than 1 (for whatever reason; I don't judge), you'll have to let your audience know via the description text or something.
- setting the range byte to 0xFF will now display Total and, as the name suggests, have total (1-FF) range. I opted to not display the squares for this option, similar to FE6's Saint's Staff and FE8's Latona, but there's some...issues.

Which brings me to...GLITCHES:

1) Total range weapons/staves will display SOME squares when selecting the target. The squares to highlight seem to be be the ones not on screen when the cursor jumps to the first target. I don't know how to fix it, and since it's (as far as I can tell) purely graphical in nature, I'm not too concerned about it. If you want to recreate Latona in FE7 (which I figure is the main reason someone would use total range), simply make a copy of Fortify with 0xFF range and this problem won't even appear (since Fortify doesn't let you pick a target).
2) Torch had a funny glitch where, if it was the only staff in your inventory, after selecting the square to move to, the displayed squares would be blue instead of green. I came up with a fix for that, but it overrode my "do not display squares if 0xFF range" function for some reason.
3) However, that's not an issue, because Torch doesn't work at 0xFF range AT ALL. I suspect the reason is because this staff is weird (I mean, they're all weird, but this one's REALLY weird), and I'm not willing to put in more time and energy into solving this unless someone really really really needs it (and you'd better have a damn good reason as to why you need it). Torch works fine at non-total ranges, and other staffs work fine (aside from the graphics glitch mentioned earlier) at total range, so...yeah. If you find a fix, lemme know.


To use this, patch FE7-Staff Range EA.txt with Event Assembler 9.12 or higher. If you've repointed the item table and/or usability table (if you don't know that is, then you didn't repoint it), be sure to update that in the definitions section. 

Credit to Icecube for his range fixing hack, which I essentially rewrote and incorporated here.

Last of all, if you want to make a custom staff, or a slightly different version of an existing one, see Notes.txt for what to change.