.thumb
.org 0x00
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@Call 22C02

	mov	r0,#4        @Fix
	str	r0,[sp]

	mov	r0,#1        @Fix
	                 @r1 = PortraitID
	mov	r2,#0xB0
	ldr r3,=0x0800563c   @NewFace
	mov lr,r3
	mov r3,#0xC
	.short 0xf800

	mov r0, #0x0
	mov r1, #0x5
	blh 0x08006458   @SetFaceBlinkControlById

	ldr r3,=0x08022C0E
	mov pc,r3
