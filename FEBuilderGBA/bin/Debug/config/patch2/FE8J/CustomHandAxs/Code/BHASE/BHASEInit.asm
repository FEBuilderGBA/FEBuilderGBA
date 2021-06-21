@ Mimics 0x5C1C8. Initializes Procs_efxTeonoSE,
@ but plays wing flap SFE instead.
.thumb
.include "../Definitions.asm"

push {r4-r7, r14}
mov   r6, r0
mov   r5, r1


ldr   r1, =gSomeSubAnim6CCounter
ldr   r0, [r1]
add   r0, #0x1
str   r0, [r1]
ldr   r0, =Procs_efxTeonoSE
mov   r1, #0x3
ldr   r7, =New6C
bl    GOTO_R7
mov   r4, r0
str   r6, [r4, #0x5C]
str   r5, [r4, #0x60]
mov   r0, #0x0
strh  r0, [r4, #0x2C]
mov   r0, #0x1
strh  r0, [r4, #0x2E]
ldr   r1, =0x100
mov   r0, #0x2
ldsh  r2, [r6, r0]

ldr   r0, [r6, #0x68]
ldrh  r0, [r0, #0x8]	@BHASFE
ldrh  r0, [r0]
mov   r3, #0x1
ldr   r7, =SomeSFERoutine
bl    GOTO_R7


mov   r0, r4
pop   {r4-r7}
pop   {r1}
bx    r1
GOTO_R7:
bx    r7
