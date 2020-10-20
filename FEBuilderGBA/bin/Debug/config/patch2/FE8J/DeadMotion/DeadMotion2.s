@Call 08053DC4	{J}
@Call 080530D4	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

bl GetMotion
@cmp r0, #0x1
@beq Normal_EfxDeadAlpha
cmp r0, #0x2
beq Smash
cmp r0, #0x3
beq Exit_NoSound
b   Normal_EfxDeadAlpha

Smash:
mov r1, r4
ldr r0, Proc_CustomDeadMotion_Smash
bl RunCustomProcs
b Exit_PlayDeadSound

@Aplhaして消去
Normal_EfxDeadAlpha:
ldr r0, [r4, #0x5c]
ldr r1, [r4, #0x60]
blh 0x08053e94   @StartEfxDeadAlpha 死亡時の点滅するエフェクト	{J}
@blh 0x080531A4   @StartEfxDeadAlpha 死亡時の点滅するエフェクト	{U}
@b PlayDeadSound

@復帰1
Exit_PlayDeadSound:
mov r1, #0x80
lsl r1 ,r1 ,#0x1
ldr r3, =0x08053DCE|1	@{J}
@ldr r3, =0x080530DE|1	@{U}
bx  r3

@復帰2
Exit_NoSound:
ldr r3, =0x08053DE0|1	@{J}
@ldr r3, =0x080530F0|1	@{U}
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

mov r0, #0xa
strh r0, [r4, #0xa]
strh r0, [r5, #0xa]
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
.set Proc_CustomDeadMotion_Smash, EALiterals + 4
