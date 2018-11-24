@
@Call  0805BD78    FE6
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

ldr   r3, =0x02023CBC   @ FE6 (BGMSTRUCT@BGM.音楽関係のフラグ1 )

ldrh  r3, [r3, #0x4]   @      (BGMSTRUCT@BGM.再生しているBGM )
cmp   r3, r4
beq   Exit
	@@mov r0 ,r2      @不要
	blh 0x08003028    @FE6 SomethingSoundRelated_80022EC

	mov r0 ,r4
	blh 0x080033c8    @FE6

Exit:
pop {r4}
pop {r0}
bx r0
