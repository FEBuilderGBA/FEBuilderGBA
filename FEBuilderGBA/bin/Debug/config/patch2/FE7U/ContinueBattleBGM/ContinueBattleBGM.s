@
@@Call  08067E8C    FE7J
@Call 080676A0    FE7U
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

@ldr   r3, =0x02024E14   @ FE7J (BGMSTRUCT@BGM.音楽関係のフラグ1 )
ldr   r3, =0x02024E1C  @ FE7U (BGMSTRUCT@BGM.音楽関係のフラグ1 )

ldrh  r3, [r3, #0x4]   @      (BGMSTRUCT@BGM.再生しているBGM )
cmp   r3, r4
beq   Exit
	@@mov r0 ,r2      @不要
	@blh 0x0800348c    @FE7J SomethingSoundRelated_80022EC
	blh 0x080035b0   @FE7U SomethingSoundRelated_80022EC

	mov r0 ,r4
	@blh 0x0800394c    @FE7J
	blh 0x08003a70   @FE7U

Exit:
pop {r4}
pop {r0}
bx r0
