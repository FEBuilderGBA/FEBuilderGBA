.equ CheckFlag, 0x8083da9
.equ Roll1RN, 0x08000ca0 + 2 + 1
.equ NextRN_100, 0x8000C64

.macro blh to, reg=r3
ldr \reg, =\to
mov lr, \reg
.short 0xF800
.endm

.thumb

push {r4,r5,lr}

push {r0}
ldrb r0, OneRNFlag
lsl r0, #0x18
lsr r0, #0x18 // clear top bytes
blh CheckFlag
mov r1, r0
pop {r0}
cmp r1, #0
beq TwoRNRoll

@ One RN Roll
ldr r1, =Roll1RN
bx r1

TwoRNRoll:
mov r5, r0
blh NextRN_100
mov r4, r0
blh NextRN_100
add r4, r0
lsr r0, r4, #0x1f
add r4, r0
asr r4, #0x1
mov r0, #0x0
cmp r5, r4
ble End
mov r0, #0x1

End:
pop {r4,r5}
pop {r1}
bx r1

.ltorg
.align

OneRNFlag:
@ BYTE Flag
