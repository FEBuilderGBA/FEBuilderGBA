.thumb
.syntax unified
.org 0x00
.equ CopyDataWithPossibleUncomp, 0x8012F50+1
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
Load6CFACEGraphics: @ 0x08005594
	push {lr}
	ldr r1, [r0, #0x2c]
	ldr r2, [r1]
	ldr r1, =0x0202A68C
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
	
	