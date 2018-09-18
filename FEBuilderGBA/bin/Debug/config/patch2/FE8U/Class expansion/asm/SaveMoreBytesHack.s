.thumb
.org 0x00

.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xF800
.endm

Player:
mov r0, #0x48
mov r2, #51
mul r2, r0 @Size of Copy
ldr r1, =0x1F78 @Dest
add r1, r7, r1
ldr r0, =0x202BE4C @Player Unit Pool
blh 0x80D1724 @SaveToSram

Enemy:
mov r0, #0x48
mov r2, #50
mul r2, r0 @Size of Copy
ldr r1, =0x1F78+(51*0x48) @Dest
add r1, r7, r1
ldr r0, =0x202CFBC @Enemy Unit Pool
blh 0x80D1724 @SaveToSram

NPC:
mov r0, #0x48
mov r2, #10
mul r2, r0 @Size of Copy
ldr r1, =0x1F78+(51*0x48)+(50*0x48) @Dest
add r1, r7, r1
ldr r0, =0x202DDCC @NPC Unit Pool
blh 0x80D1724 @SaveToSram

ldr r3, =0x80A5B11
bx r3

.pool
