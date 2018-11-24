@
@@Call  08073F4C    FE8J
@Call 08071A68    FE8U
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

@ldr   r3, =0x02024E5C   @ FE8J (BGMSTRUCT@BGM.音楽関係のフラグ1 )
ldr   r3, =0x02024E5C  @ FE8U (BGMSTRUCT@BGM.音楽関係のフラグ1 )

ldrh  r3, [r3, #0x4]   @      (BGMSTRUCT@BGM.再生しているBGM )
cmp   r3, r4
beq   Exit
	@@mov r0 ,r2      @不要
	@blh 0x0800223c    @FE8J SomethingSoundRelated_80022EC
	blh 0x080022ec   @FE8U SomethingSoundRelated_80022EC

	mov r0 ,r4
	@blh 0x08002570    @FE8J
	blh 0x08002620   @FE8U

Exit:
pop {r4}
pop {r0}
bx r0
