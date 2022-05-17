@Hook 080589C2	@{J}
@Hook 08057A7A	@{U}
.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

push {r4}
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

mov	r3, r10

CheckUnit:
ldrb	r0, [r4]	@Table->Unit
cmp		r0, #0x00	@ANY
beq		CheckMap

ldr		r1, [r3, #0x0]
ldrb	r1, [r1, #0x4]
cmp		r0, r1
bne		Loop

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
ldrb	r0, [r4,#0x2]	@Table->Cond
cmp		r0, #0xFF	@ANY
beq		Found

cmp		r0, #0x1
beq		BossAnimeOn_BossFlag

cmp		r0, #0x2
beq		BossAnimeOn_BossFlagAndEnmey

cmp		r0, #0x3
beq		BossAnimeOn_BossFlagAndNPC

cmp		r0, #0x4
beq		BossAnimeOn_ENEMY

cmp		r0, #0x5
bne		Loop

BossAnimeOn_NPC:
ldrb	r0, [r3, #0xB]
mov		r1, #0x40
and		r0, r1
beq		Loop
b		Found

BossAnimeOn_ENEMY:
ldrb	r0, [r3, #0xB]
mov		r1, #0x80
and		r0, r1
beq		Loop
b		Found

BossAnimeOn_BossFlagAndNPC:
ldrb	r0, [r3, #0xB]
mov		r1, #0x40
and		r0, r1
beq		Loop
b		BossAnimeOn_BossFlag

BossAnimeOn_BossFlagAndEnmey:
ldrb	r0, [r3, #0xB]
mov		r1, #0x80
and		r0, r1
beq		Loop
@b		BossAnimeOn_BossFlag


BossAnimeOn_BossFlag:
ldr		r0, [r3, #0]
ldrh	r0, [r0, #0x28]
lsl		r0, r0, #0x10
lsr		r0, r0, #0x1f
beq		Loop

Found:
ldrb	r0, [r4,#0x3]	@Table->Anime
cmp		r0, #0x1		@BossAnimeOn_ANIME_1
beq		AnimeOn

cmp		r0, #0x2		@BossAnimeOn_ANIME_2
beq		AnimeOn

@cmp		r0, #0xFF		@BossAnimeOn_ANIME_OFF
@beq		AnimeOff

Default:
pop {r4}
ldr r3, =0x080589DA|1	@{J}
@ldr r3, =0x08057A92|1	@{U}
bx  r3

AnimeOff:
pop {r4}
ldr r3, =0x080589FE|1	@{J}
@ldr r3, =0x08057AB6|1	@{U}
bx  r3

AnimeOn:
pop {r4}
ldr r3, =0x080589FC|1	@{J}
@ldr r3, =0x08057AB4|1	@{U}
bx  r3

.align 4
.ltorg
BossAnimeOnTable:
