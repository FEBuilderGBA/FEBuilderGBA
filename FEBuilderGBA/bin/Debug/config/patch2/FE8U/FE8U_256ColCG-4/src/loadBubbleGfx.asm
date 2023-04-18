@ Load textbubble palette and gfx to OBJPAL and OBJVRAM.
@ Hooked at 0x8EB10.
.thumb

mov   r5, r0            @ ProcState. Need to safekeep.

@ Load textbubble palette.
ldr   r0, =0x89E84D4
mov   r1, #0x98
lsl   r1, #0x2
mov   r2, #0x20
ldr   r4, =CopyToPaletteBuffer
bl    GOTO_R4

@ Load textbubble gfx.
ldr   r0, =CGC_BubbleSpriteGfx
ldr   r1, =gGenericBuffer
ldr   r4, =Decompress
bl    GOTO_R4
ldr   r0, =gGenericBuffer
ldr   r1, =0x6013000
mov   r2, #0x9
mov   r3, #0x4
ldr   r4, =CopyTileGfxForObj
bl    GOTO_R4

@ Vanilla, overwritten by hook.
mov   r3, r5
add   r2, r13, #0x18
ldr   r1, [r3, #0x2C]   @ textbuffer.
ldrb  r0, [r1]          @ First char.

ldr   r4, =0x808EB19
GOTO_R4:
bx    r4
