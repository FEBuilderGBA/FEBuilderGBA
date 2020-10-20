@Call 08053D58	{J}
@Call 08053068	{U}


.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

bl GetMotion
cmp r0, #0x1
beq Flush_And_EfxDeadPika
cmp r0, #0x2
beq Nothing
cmp r0, #0x3
beq Flush_Only
b   Normal_EfxDeadPika

@何もしない
Nothing:
mov r0, r5
bl SetNothing
b Exit

@画面フラッシュ
Flush_Only:
mov r1, r5
ldr r0, Proc_CustomDeadMotion_Flush_Only
bl RunCustomProcs
b Exit

@画面フラッシュした後で点滅して消去
Flush_And_EfxDeadPika:
mov r1, r5
ldr r0, Proc_CustomDeadMotion_Flush_And_Pika
bl RunCustomProcs
b Exit

@点滅して消去
Normal_EfxDeadPika:
ldr r0, [r5, #0x5c]
ldr r1, [r5, #0x60]
blh 0x08053e10   @StartEfxDeadPika	{J}
@blh 0x08053120   @StartEfxDeadPika	{U}
@b Exit

@復帰
Exit:
ldr r3, =0x08053D60|1	@{J}
@ldr r3, =0x08053070|1	@{U}
bx  r3

RunCustomProcs:
push {r4,r5,lr}
ldr r4, [r1, #0x5c]
ldr r5, [r1, #0x60]
blh 0x08002c30   @NewBlocking6C	{J}
@blh 0x08002CE0   @NewBlocking6C	{U}
str r4, [r0, #0x5c]
str r5, [r0, #0x60]
mov r1, #0x0
strh r1, [r0, #0x2c]
strh r1, [r0, #0x2e]
pop {r4,r5}
pop {r1}
bx r1

SetNothing:
push {r4,r5,lr}
mov r1, #0x0
strh r1, [r0, #0x2e]

mov  r1, #0x1e-1
strh r1,[r0,#0x2c]

pop {r4,r5}
pop {r1}
bx r1

GetMotion:
push {lr,r4}
ldr   r4,=0x0203a568	@BattleUnit	gBattleTarget	{J}
@ldr   r4,=0x0203A56C	@BattleUnit	gBattleTarget	{U}
ldrb r0, [r4, #0xb]
cmp  r0, #0x80
bge GetMotion_Find	@敵のみ

ldr   r4,=0x0203a4e8	@BattleUnit	gBattleActor	{J}
@ldr   r4,=0x0203A4EC	@BattleUnit	gBattleActor	{U}
ldrb r0, [r4, #0xb]
cmp  r0, #0x80
bge GetMotion_Find	@敵のみ
b   GetMotion_NotFound

GetMotion_Find:
ldr r3, DeadMotionTable
sub r3, #0x4
GetMotion_Loop:
add r3, #0x4
ldr r0, [r3]
cmp r0, #0x0
beq GetMotion_NotFound

GetMotion_CheckUnit:
ldrb r0, [r3,#0x0]
cmp  r0, #0x0
beq  GetMotion_CheckClass

ldr  r1, [r4,#0x0]
ldrb r1, [r1,#0x4]
cmp  r0, r1
bne  GetMotion_Loop

GetMotion_CheckClass:
ldrb r0, [r3,#0x1]
cmp  r0, #0x0
beq  GetMotion_CheckMap

ldr  r1, [r4,#0x4]
ldrb r1, [r1,#0x4]
cmp  r0, r1
bne  GetMotion_Loop

GetMotion_CheckMap:
ldrb r0, [r3,#0x2]
cmp  r0, #0xff
beq  GetMotion_Found

ldr  r2, =0x0202BCEC	@ChapterData	{J}
@ldr  r2, =0x0202BCF0	@ChapterData	{U}
ldrb r1, [r2,#0xE]	@MapID
cmp  r0, r1
bne  GetMotion_Loop

GetMotion_Found:
ldrb r0, [r3,#0x3]
b    GetMotion_Exit

GetMotion_NotFound:
mov r0, #0x0

GetMotion_Exit:
pop {r4}
pop {r1}
bx r1


.ltorg
EALiterals:
.set DeadMotionTable, EALiterals + 0
.set Proc_CustomDeadMotion_Flush_And_Pika, EALiterals + 4
.set Proc_CustomDeadMotion_Flush_Only, EALiterals + 8
