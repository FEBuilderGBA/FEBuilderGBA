.thumb
.align
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.equ gActionData,0x0203A85C
.equ GetUnit,0x08018D0C+1
.equ SetUnitStatus,0x08017600+1
.equ gBattleStats,0x0203A3D8
.equ FinishUpItemBattle,0x0802A5EC+1
.equ BeginBattleAnimations,0x0802A3B0+1

@located at 0802CFDC
push {r7}
ldr r0,=gActionData
ldrb r0,[r0,#0xD]
blh GetUnit,r7
mov r1,r5
mov r2,r1
blh SetUnitStatus,r7
sub r0,#0x30
ldr	r1,[r0,#0xC]
ldr	r2,=0x802F50C
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

