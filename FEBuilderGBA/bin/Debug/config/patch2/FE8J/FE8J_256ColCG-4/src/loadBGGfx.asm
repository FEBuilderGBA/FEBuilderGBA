@ Loads 256 or 224 colour BG if BG TSA pointer is 0 or 1 respectively.
@ Hooked at 0xE844.
.thumb

@ Vanilla, overwritten by hook.
lsl   r4, r6, #0x1
add   r4, r6
lsl   r4, #0x2
add   r0, r4, r5                @ Arg0 contains address of BGStruct.

mov   r1, #0x80
lsl   r1, #0x1                  @ Arg1 = 256. Indicates how many colours BG uses.
ldr   r2, [r0, #0x4]
cmp   r2, #0x0                  @ Check TSA pointer against 0.
beq   LoadMultiPalBG
  sub   r1, #0x20               @ Arg1 = 224. Indicates how many colours BG uses.
  cmp   r2, #0x1                @ Check TSA pointer against 1.
  beq   LoadMultiPalBG
    ldr   r1, =0x800E84D        @ Neither 256 nor 224 colour BG, run vanilla.
    bx    r1

LoadMultiPalBG:
ldr   r2, =CGC_LoadMultiPalBG
bl    GOTO_R2
ldr   r2, =0x800E8BF            @ Return to end of caller.

GOTO_R2:
bx    r2
