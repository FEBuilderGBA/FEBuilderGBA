@
@Call  08073F4C    FE8J
@@Call 08071A68    FE8U
@
@r0 Nazo
@
@
@r4 Change SongID
@
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb

@もし、0xFFFF を戦闘BGMとして指定した場合は、絶対に切り替えない
ldr r3, =0xFFFF
cmp r3, r4
beq SomeBGM

@音楽の優先度が0x03以下だったら効果音として再生する
@ldr   r3, =0x080D5024	@FE8J SoundRoomPointer
ldr   r3, =0x080028BC	@FE8U SoundRoomPointer
ldr   r3, [r3]
lsl   r2, r4, #0x3		@ songID*4
add   r3, r2
ldrb  r2, [r3, #0x4]
cmp   r2, #0x3
blt   CheckSameBGM

mov   r0, r4
@blh   0x080d4ef4	@FE8J m4aSongNumStart	効果音の再生
blh   0x080d01fc	@FE8U m4aSongNumStart	効果音の再生
b     SomeBGM

CheckSameBGM:
@ldr   r3, =0x02024E5C   @ FE8J (BGMSTRUCT@BGM.音楽関係のフラグ1 )
ldr   r3, =0x02024E5C    @ FE8U (BGMSTRUCT@BGM.音楽関係のフラグ1 )

ldrh  r3, [r3, #0x4]   @      (BGMSTRUCT@BGM.再生しているBGM )
cmp   r3, r4
beq   SomeBGM
	@@mov r0 ,r2      @不要
@	blh 0x0800223c    @FE8J SomethingSoundRelated_80022EC
	blh 0x080022ec    @FE8U SomethingSoundRelated_80022EC

	mov r0 ,r4
@	blh 0x08002570    @FE8J
	blh 0x08002620   @FE8U
	b Exit

SomeBGM:
@	ldr   r3, =0x02024E5C   @ FE8J (BGMSTRUCT@BGM.音楽関係のフラグ1 )
	ldr   r3, =0x02024E5C   @ FE8U (BGMSTRUCT@BGM.音楽関係のフラグ1 )
	@待機BGMを0にする
	mov   r0, #0x0
	strh  r0, [r3, #0x2]
Exit:
pop {r4}
pop {r0}
bx r0
