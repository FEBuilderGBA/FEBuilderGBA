.thumb
.include "../Definitions.asm"

push {r4-r7, r14}
mov   r5, r0  @BHA   Procs

ldr   r0, =BHAOBJProcs
mov   r1, #0x3
ldr   r7, =New6C
bl    GOTO_R7
mov   r4, r0

ldr   r0, [r5, #0x5C]
str   r0, [r4, #0x5C]
ldr   r0, [r5, #0x60]
str   r0, [r4, #0x60]
ldr   r0, [r5, #0x64]
str   r0, [r4, #0x64]
ldr   r0, [r5, #0x68]
str   r0, [r4, #0x68]

mov   r0, #0x0
strh  r0, [r4, #0x2C]

@ Determine time based on ranged or melee
ldr   r0, =AnimDistance
mov   r1, #0x0
ldsh  r0, [r0, r1]
cmp   r0, #0x0
bne   L7
  ldr   r0, =BHAMeleeTime
  b     L8
L7:
  ldr   r0, =BHARangedTime
L8:
ldrb  r0, [r0]
strh  r0, [r4, #0x2E]

mov   r0, r4
pop   {r4-r7}
pop   {r1}
bx    r1
GOTO_R7:
bx    r7
