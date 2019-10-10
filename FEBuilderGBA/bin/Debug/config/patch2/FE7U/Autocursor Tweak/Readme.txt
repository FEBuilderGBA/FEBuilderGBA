To install the autocursor tweak:
-Paste write the contents of "Cursor.dmp" to some free space in the ROM, at a word-aligned offset (divisible by 4)
-At 0x1D64C, paste write this: 004A 1047 XXXXXXXX, where XXXXXXXX is a pointer to where you pasted the code +1.
So if you pasted the code to 0xD20000, write 0100D208.

To install the status screen tweak:
-Paste write the contents of "Status Screen.dmp" to some free space in the ROM, at a word-aligned offset.
-At 0x8681C, paste write this: 004A 1047 XXXXXXXX, where XXXXXXXX is a pointer to where you pasted the code +1.
So if you pasted the code to 0xD20100, write 0101D208.

These two together will make the autocursor target the first deployed character and make the status screen display
that same character as your party's leader, also preventing you from zooming to a missing lord.