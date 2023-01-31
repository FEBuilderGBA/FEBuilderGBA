@Hook 080589C2	@{J}
@Hook 08057A7A	@{U}
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

push {r4, r5}
CMP r0, #0x0
BEQ AnimeOn

CMP r0, #0x3
BEQ AnimeOn

CMP r0, #0x1
BNE AnimeOff

ldr r4, BossAnimeOnTable
sub r4, #0x4

Loop:
add r4, #0x4
ldr r0, [r4]
cmp r0, #0x0
beq Default

CheckMap:
ldrb	r0, [r4,#0x1]	@Table->Map
cmp		r0, #0xFF	@ANY
beq		CheckCond

ldr		r1, =0x0202bcec @ChapterData	@{J}
@ldr	r1, =0x0202bcf0 @ChapterData	@{U}
ldrb	r1, [r1, #0xE]	@ChapterData->MAPID
cmp		r0, r1
bne		Loop

CheckCond:
mov r5, r10	@{J}
@mov r5, r9	@{U}
blhh Boss_Animation_ON_Check
cmp r0, #0x0
beq Loop

@ldr r5, =0x0203A4E8	@gBattleActor @{J}
@@ldr r5, =0x0203A4EC	@gBattleActor @{U}
@blhh Boss_Animation_ON_Check
@cmp r0, #0x1
@beq Found
@
@ldr r5, =0x0203A568	@gBattleTarget @{J}
@@ldr r5, =0x0203A56C	@gBattleTarget @{U}
@blhh Boss_Animation_ON_Check
@cmp r0, #0x1
@bne Loop

Found:
ldrb	r0, [r4,#0x3]	@Table->Anime
cmp		r0, #0x1		@BossAnimeOn_ANIME_1
beq		AnimeOn

cmp		r0, #0x2		@BossAnimeOn_ANIME_2
beq		AnimeOn

@cmp		r0, #0xFF		@BossAnimeOn_ANIME_OFF
@beq		AnimeOff

Default:
pop {r4, r5}
ldr r3, =0x080589DA|1	@{J}
@ldr r3, =0x08057A92|1	@{U}
bx  r3

AnimeOff:
pop {r4, r5}
ldr r3, =0x080589FE|1	@{J}
@ldr r3, =0x08057AB6|1	@{U}
bx  r3

AnimeOn:
pop {r4, r5}
ldr r3, =0x080589FC|1	@{J}
@ldr r3, =0x08057AB4|1	@{U}
bx  r3

.align
.ltorg
BossAnimeOnTable:
.equ Boss_Animation_ON_Check,	BossAnimeOnTable+4
