First things first, put these in your ParseDefinitions.txt. You don't technically need to, but it's a lot easier to remember than [0x90], right?

[ConversationText] = [0x90]
[BattleText] = [0x91]

Then, check out these text entries that explain how the hack works. Feel free to insert them into your game if you want to see how the hack functions.
Though things generally look nicer if you keep sentences short enough that they can fit inside a single text box (which these examples don't).

## TEXT_WelcomeToAutoNewline
[ConversationText]
[MidLeft][LoadEphraim][Right][LoadEirika]
[MidLeft]
Hello. Ephraim and Eirika here.
We'll be explaining how AutoNewline works.
[Right]
First off, notice the 'ConversationText' code at the start of this text entry?
That's necessary to activate the hack.
It tells the hack what kind of text to expect.
If you leave it off, the hack won't activate and you'll get vanilla text processing.
The currently supported types of text are ConversationText and BattleText.
They function the same, except that each has a different maximum width.
This is because battle conversations have less room for text.
[MidLeft]
And, as I'm sure you've already noticed, there are no pesky 'A' or 'N' codes here.
You just type like normal, and the hack takes care of the rest!
Furthermore, you don't even need to use that old 'Beat' macro either.
Just type your ... and the hack takes care of everything for you.
It even automatically inserts ToggleMouthMove around --, too!
[Right]
By the way, there are several options in the installer that control how the hack acts.
Each one comes with comments describing it, so check them out.
Or just leave everything at default, that works too.
[MidLeft]
Now, things that currently aren't supported.
First - world map text.
The hack creator may consider it in the future, but as the vast majority of hacks don't use the world map, it's fairly low priority.
Secondly - misc text. Things like skill descriptions, character/class descriptions, other things on the statscreen, etc.
Support for this may or may not be added in the future.
Finally, only regular ASCII text is supported - and the punctuation handling will not currently function on punctuation that isn't present in English.
[Right]
Oh...![AN]
Before I forget, you can still use the A and N text codes manually if you so desire.
Sometimes you might want to have something on its own line for dramatic purposes.
The hack supports this just fine.
Finally, the hack only works on anti-huffman'd text.
Happy text writing![X]


## TEXT_BattleText
[BattleText]
[MidLeft][LoadONeill]
[MidLeft]
Here's an example of BattleText.
[MidLeft][ClearFace][MidRight][LoadEirika]
[MidRight]
It's not really much different from ConversationText.
Except that you have lower maximum width.
Because the mugs need to fit alongside the text.
[MidRight][ClearFace][MidLeft][LoadONeill]
[MidLeft]
But it's kind of a pain having to manually clear face and reload so much, isn't it?
[MidLeft][ClearFace][MidRight][LoadEirika]
[MidRight]
No promises, but I heard a rumor this might be addressed in a future version of the hack.[X]