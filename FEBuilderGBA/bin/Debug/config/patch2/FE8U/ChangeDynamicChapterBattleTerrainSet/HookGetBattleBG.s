@HOOK FE8J	5899C	{J}
@HOOK FE8U	57A54	{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4}
mov  r4, r0 @keep MapID

FindBattleBGObject:
@ldr r0, =0x0203A610	@TrapData	@{J}
ldr r0, =0x0203A614	@TrapData	@{U}
ldr r3,=0x40*8
add r3,r0				@Term

Loop:
ldrb r1,[r0,#0x2] @Trap->Type
cmp  r1,#0xED			@TrapType 0xED
beq  Found
add  r0, #0x8

cmp  r0,r3
blt  Loop

@見つからないので、通常の戦闘背景
ExitNotFound:
mov r0, r4
@blh 0x08034520   @マップ番号から、マップ設定のアドレスを返す関数 GetROMChapterStruct RET=マップ設定のアドレス r0=調べたいマップID:MAPCHAPTER	{J}
blh 0x08034618   @マップ番号から、マップ設定のアドレスを返す関数 GetROMChapterStruct RET=マップ設定のアドレス r0=調べたいマップID:MAPCHAPTER	{U}
ldrb r1, [r0, #0x13]	@ r0=マップ設定のアドレス
b   ExitContinueVanilla

Found:
ldrb r1, [r0,#0x4]	@CurrentTrap->Trap.Data1
@b   ExitContinueVanilla


ExitContinueVanilla:
pop {r4}

@ldr r0,=0x80589A6|1	@{J}
ldr r0,=0x8057A5E|1	@{U}
bx  r0
