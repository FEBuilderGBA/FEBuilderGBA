@Hook 0805896C	@{J}
@Hook 08057A24	@{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.macro blhh to, reg=r3
  ldr \reg, \to
  mov lr, \reg
  .short 0xf800
.endm

push {r4,r5}
@blh 0x0802c9d0   @GetBattleAnimType	@{J}
blh 0x0802ca98   @GetBattleAnimType	@{U}
cmp r0, #0x3
beq BattleBGON

ldr r4, BossAnimeOnTable
sub r4, #0x4

Loop:
add r4, #0x4
ldr r0, [r4]
cmp r0, #0x0
beq BattleBGOff

CheckMap:
ldrb	r0, [r4,#0x1]	@Table->Map
cmp		r0, #0xFF	@ANY
beq		CheckCond

@ldr		r1, =0x0202bcec @ChapterData	@{J}
ldr		r1, =0x0202BCF0 @ChapterData	@{U}
ldrb	r1, [r1, #0xE]	@ChapterData->MAPID
cmp		r0, r1
bne		Loop

CheckCond:
@mov r5, r10	@{J}
mov r5, r9	@{U}
blhh Boss_Animation_ON_Check
cmp r0, #0x0
beq Loop

Found:
ldrb	r0, [r4,#0x3]	@Table->Anime
cmp		r0, #0x2		@BossAnimeOn_ANIME_2
beq		BattleBGON

BattleBGOff:
pop {r4,r5}
@ldr r3, =0x080589ae|1	@{J}
ldr r3, =0x08057a66|1	@{U}
bx  r3

BattleBGON:
pop {r4,r5}
@ldr r3, =0x08058974|1	@{J}
ldr r3, =0x08057A2C|1	@{U}
bx  r3

.align
.ltorg
BossAnimeOnTable:
.equ Boss_Animation_ON_Check,	BossAnimeOnTable+4
