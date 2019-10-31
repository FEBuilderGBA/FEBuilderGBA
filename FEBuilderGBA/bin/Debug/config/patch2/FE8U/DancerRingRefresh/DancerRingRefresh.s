.thumb
.align
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.equ gActionData,0x203A958
.equ GetUnit,0x8019431
.equ SetUnitStatus,0x80178F5
.equ gBattleStats,0x203A4D4
.equ FinishUpItemBattle,0x802CC55
.equ BeginBattleAnimations,0x802CA15

@located at 802FC18
push {r7}
ldr r0,=gActionData
ldrb r0,[r0,#0xD]
blh GetUnit,r7
mov r1,r5
mov r2,r1
blh SetUnitStatus,r7
sub r0,#0x30
ldr	r1,[r0,#0xC]
ldr	r2,=#0x80323A0
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

