.thumb
.align
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.equ gActionData,0x0203A954
.equ GetUnit,0x08019108|1
.equ SetUnitStatus,0x0801769C|1
.equ gBattleStats,0x0203A4D0
.equ FinishUpItemBattle,0x0802CB8C|1
.equ BeginBattleAnimations,0x0802C94C|1

@located at 802FB68	{J}
push {r7}
ldr r0,=gActionData
ldrb r0,[r0,#0xD]
blh GetUnit,r7
mov r1,r5
mov r2,r1
blh SetUnitStatus,r7
sub r0,#0x30
ldr	r1,[r0,#0xC]
@ldr	r2,=0x80323A0	@{U}
ldr	r2,=0x80322EC	@{J}
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

