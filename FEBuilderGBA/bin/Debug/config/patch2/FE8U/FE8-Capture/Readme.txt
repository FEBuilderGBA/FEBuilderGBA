FE8-Capture README
By Tequila

How to use:
Either assemble FE8-Capture Master EA File.txt directly with Event Assembler 10.1 or higher, or #include it in your buildfile. Update the necessary labels (Free_Space and Item_Table). You'll also have to change the text ids for both Capture and its R-button description yourself; currently, they're set to those for Secret.
' Capture' (the space in front is important!)
'Strike a fatal blow with reduced
stats to capture an enemy.'

What does it do?
Allows someone to capture an enemy, similar to FE5, provided that
1) The capturer would be able to rescue the capturee normally (Aid >= Con)
2) The capturer isn't currently rescuing anyone
3) The capturer uses a weapon that can attack at 1 range (sorry archers).

- For instance, a javelin or hand axe can only be used to capture at 1 range.
- Drawbacks: All stats aside from HP, Con, and Mov are halved when capturing. This can be changed by opening FE8-Capture/FE8-Capture EA.txt, scrolling down to line 31, and commenting out the skills that you don't want to halve (for instance, you might only want to halve offensive skills and leave defense and resistance alone).
- After the battle, the enemy will flicker and die as normal, but will be rescued by the attacker. The only way to check the captured enemy's stats/inventory is to open the stats page of the capturer and press A.
- Any allied unit can trade with a captured unit (that is, after all, the entire point of this exercise).
- While capturing, you will suffer the standard halved skill/speed penalty.
- If you drop the unfortunate victim, they will disappear. THIS CANNOT BE REVERSED! Make sure you steal their stuff before dropping them.

Bugs:
- When selecting Capture from the menu, the red squares that would normally highlight its range don't appear. They do appear when selecting weapons, however. Just know that you can only capture in 1 range (currently).
- It's possible that there may be some glitches with saving enemy character data (although there shouldn't be). I had to fiddle with the function that removes deceased non-player character data, so that may have consequences down the line. Let me know if this is the case.
- (Not really a bug, more of a warning) This hack will either break or otherwise not play nice with other hacks that modify the stat getters (examples include debuffs from FE:SS or Venno's passive item boost hack) or modify range stuff (my staff range hack).

FAQ:
- I don't like the con/1-range restrictions. Can I modify your code?
  Yes, absolutely. All the source code is included here, and you can ask me if you're confused about something.
- I don't like the con/1-range restrictions. Can you modify this to my specifications?
  Maybe, if I like you enough. Probably not, though.
- Is there an FE7 version?
  No, and I don't intend to make one, unless someone really really really wants it and has an fe7 project that's already well-developed. If that's not the case, switch to 8. It's better. [/fe8_shilling]
- Are you remaking FE5 in FE8?
  No.