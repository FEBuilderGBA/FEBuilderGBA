.thumb
.org 0x00

.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xF800
.endm
Init:
ldr r5, =0x30067A0

Player:
mov r0, #0x48
mov r2, #51
mul r2, r0 @Size of Copy
ldr r0, =0x1F78 @Dest
add r0, r6, r0
ldr r1, =0x202BE4C @Player Unit Pool
ldr r3, [r5] @ LoadFromSRAM
bl BXR3

Enemy:
mov r0, #0x48
mov r2, #50
mul r2, r0 @Size of Copy
ldr r0, =0x1F78+(51*0x48) @Dest
add r0, r6, r0
ldr r1, =0x202CFBC @Enemy Unit Pool
ldr r3, [r5] @ LoadFromSRAM
bl BXR3

NPC:
mov r0, #0x48
mov r2, #10
mul r2, r0 @Size of Copy
ldr r0, =0x1F78+(51*0x48)+(50*0x48) @Dest
add r0, r6, r0
ldr r1, =0x202DDCC @NPC Unit Pool
ldr r3, [r5] @ LoadFromSRAM
bl BXR3

ldr r3, =0x80A5CB1

BXR3:
bx r3

.pool
