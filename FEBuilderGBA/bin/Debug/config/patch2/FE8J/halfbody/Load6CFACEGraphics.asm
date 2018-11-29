.thumb
.syntax unified
.org 0x00
@.equ CopyDataWithPossibleUncomp, 0x8012F50+1 @FE8U
.equ CopyDataWithPossibleUncomp, 0x8013008+1  @FE8J
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
Load6CFACEGraphics: @ 0x0800549C(FE8J) @ 0x08005594(FE8U)
	push {lr}
	ldr r1, [r0, #0x2c]
	ldr r2, [r1]
@	ldr r1, =0x0202A68C	@FE8U
	ldr r1, =0x0202A688	@FE8J
	adds r0, #0x40
	ldrb r0, [r0]
	cmp r0, #1
	bne Continue
	adds r0, #1
	Continue:
	lsls r0, r0, #3
	adds r0, r0, r1
	ldr r1, [r0]
	ldr r0, =0x06010000
	adds r1, r1, r0
	adds r0, r2, #0
	blh CopyDataWithPossibleUncomp
	pop {r0}
	bx r0
	.align 2, 0
	.pool
	
	