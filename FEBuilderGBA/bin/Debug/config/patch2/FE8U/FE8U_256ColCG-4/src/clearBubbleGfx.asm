@ Clear textbubble OBJVRAM. This is necessary as the sprite is still displayed
@ for 1 frame after having been blended out and BLDCNT getting reset.
@ Hooked at 0x8F088.
.thumb

push  {r14}
sub   sp, #0x4

@ Clear textbubble OBJVRAM.
mov   r0, #0x0
str   r0, [sp]
mov   r0, sp
ldr   r1, =0x6013000
mov   r2, #0x1
lsl   r2, #0x18           @ FILL flag set.
mov   r3, #0x80
lsl   r3, #0x3            @ 0x400 words, meaning 0x1000 bytes.
orr   r2, r3
swi   #0xC                @ CpuFastSet.

@ Vanilla, overwritten by hook.
ldr   r1, =0x808EA3D      @ Gets some flags.
bl    GOTO_R1
mov   r1, r0
mov   r0, #0xC0
lsl   r0, #0x8
and   r0, r1

add   sp, #0x4
pop   {r1}
GOTO_R1:
bx    r1
