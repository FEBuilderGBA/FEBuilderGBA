.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ArenaDicHookArenaAfter:
push {lr, r4, r5, r6}

@壊すコードの再送
blh 0x8013E44|1	@StartFadeOutBlackMedium	@{J}

blh 0x08031E24	@闘技場の試合結果の取得	@{J}
@blh 0x08031ed8	@闘技場の試合結果の取得	@{U}
cmp r0, #0x1
bne Exit


ldr r3, =0x0203E108	@BattleAnime1
ldrh r5, [r3]   @直前に戦った相手の戦闘アニメ
add r5, #0x01

mov r6, #0x0 @counter
ldr r4, ArenaDicStruct
Loop:
ldr r0, [r4, #0x0]
cmp r0, #0x0
beq Exit

ldrh r0, [r4, #0x0E]	@ArenaDicStruct->BattleAnimeID
cmp r0, r5
bne Next

Found:
mov r0, r6
bl  SetBit

Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r6, #0x01
b   Loop

Exit:
pop {r4,r5,r6}
pop {r0}
bx r0

SetBit:
push {lr}
@convert to bitflag
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, ArenaDicConfig
ldr r3, [r3, #0x1C]	@ArenaDicConfig->RAM

ldr r0, [r3, r2]
and r0 ,r1
cmp r0, #0x0
bne SetBit_Exit  @既にON

@フラグを立てる
ldr r0, [r3, r2]
orr r0 ,r1
str r0, [r3, r2]

SetBit_Exit:
pop {r0}
bx r0


.ltorg
DATA:
.equ	ArenaDicStruct,	DATA+0
.equ	ArenaDicConfig,	DATA+4
