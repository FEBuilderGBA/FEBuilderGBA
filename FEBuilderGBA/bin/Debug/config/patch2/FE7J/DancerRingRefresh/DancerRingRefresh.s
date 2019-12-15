.thumb
.align
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.equ gActionData,0x0203A858
.equ GetUnit,0x0080190F4+1
.equ SetUnitStatus,0x080179F0+1
.equ gBattleStats,0x0203A3D4
.equ FinishUpItemBattle,0x0802AA9C+1
.equ BeginBattleAnimations,0x0802A860+1

@located at 0802D49C
push {r7}
ldr r0,=gActionData
ldrb r0,[r0,#0xD]
blh GetUnit,r7
mov r1,r5
mov r2,r1
blh SetUnitStatus,r7
sub r0,#0x30
ldr	r1,[r0,#0xC]
ldr	r2,=0x0802F9D8
ldr	r2,[r2]
and	r1,r2
str	r1,[r0,#0xC]
ldr r1,=gBattleStats
mov r0,#0x80
lsl r0,r0,#2
strh r0,[r1]
mov r0,r6
blh FinishUpItemBattle,r7
blh BeginBattleAnimations,r7
pop {r7}
pop {r4-r6}
pop {r0}
bx r0

.ltorg
.align

