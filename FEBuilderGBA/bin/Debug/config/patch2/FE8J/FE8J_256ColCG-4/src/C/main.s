	.cpu arm7tdmi
	.eabi_attribute 20, 1
	.eabi_attribute 21, 1
	.eabi_attribute 23, 3
	.eabi_attribute 24, 1
	.eabi_attribute 25, 1
	.eabi_attribute 26, 1
	.eabi_attribute 30, 4
	.eabi_attribute 34, 0
	.eabi_attribute 18, 4
	.file	"main.c"
	.text
	.align	1
	.global	CGC_LoadMultiPalBG
	.arch armv4t
	.syntax unified
	.code	16
	.thumb_func
	.fpu softvfp
	.type	CGC_LoadMultiPalBG, %function
CGC_LoadMultiPalBG:
	@ Function supports interworking.
	@ args = 0, pretend = 0, frame = 8
	@ frame_needed = 0, uses_anonymous_args = 0
	movs	r2, #0
	push	{r0, r1, r4, r5, r6, lr}
	ldr	r6, .L7
	movs	r4, r0
	movs	r5, r1
	movs	r0, r2
	movs	r1, r2
	bl	.L9
	movs	r2, #0
	movs	r0, #1
	movs	r1, r2
	bl	.L9
	movs	r2, #0
	movs	r0, #2
	movs	r1, r2
	bl	.L9
	movs	r2, #0
	movs	r0, #3
	movs	r1, r2
	bl	.L9
	movs	r1, #224
	ldr	r6, .L7+4
	movs	r0, #0
	lsls	r1, r1, #8
	bl	.L9
	movs	r1, #232
	movs	r0, #1
	lsls	r1, r1, #8
	bl	.L9
	movs	r1, #240
	movs	r0, #2
	lsls	r1, r1, #8
	bl	.L9
	movs	r1, #248
	movs	r0, #3
	lsls	r1, r1, #8
	bl	.L9
	movs	r1, #0
	ldr	r6, .L7+8
	movs	r0, r1
	bl	.L9
	movs	r1, #0
	movs	r0, #1
	bl	.L9
	movs	r1, #0
	movs	r0, #2
	bl	.L9
	movs	r0, #3
	movs	r1, #0
	bl	.L9
	movs	r2, #128
	ldr	r3, .L7+12
	ldrb	r1, [r3, #24]
	rsbs	r2, r2, #0
	orrs	r2, r1
	movs	r1, #32
	strb	r2, [r3, #24]
	adds	r3, r3, #60
	ldrb	r2, [r3, #1]
	bics	r2, r1
	strb	r2, [r3, #1]
	mov	r3, sp
	movs	r6, #0
	adds	r0, r3, #2
	ldr	r2, .L7+16
	ldr	r1, .L7+20
	ldr	r3, .L7+24
	strh	r6, [r0]
	bl	.L10
	movs	r1, #192
	ldr	r2, .L7+28
	ldr	r3, .L7+32
	lsls	r1, r1, #19
	add	r0, sp, #4
	str	r6, [sp, #4]
	bl	.L10
	ldr	r1, .L7+36
	ldr	r3, .L7+40
	ldr	r0, [r4]
	bl	.L10
	movs	r3, #128
	movs	r1, #224
	ldr	r2, .L7+44
	lsls	r3, r3, #1
	lsls	r1, r1, #2
.L2:
	strh	r3, [r2]
	adds	r3, r3, #1
	lsls	r3, r3, #16
	lsrs	r3, r3, #16
	adds	r2, r2, #2
	cmp	r3, r1
	bne	.L2
	ldr	r0, [r4, #8]
	ldr	r6, .L7+48
	cmp	r5, #224
	bne	.L3
	movs	r2, #64
	movs	r1, #0
	bl	.L9
	movs	r2, #192
	movs	r1, #128
	ldr	r0, [r4, #8]
	lsls	r2, r2, #1
	adds	r0, r0, #64
.L6:
	bl	.L9
	@ sp needed
	movs	r0, #15
	ldr	r3, .L7+52
	bl	.L10
	ldr	r3, .L7+56
	bl	.L10
	pop	{r0, r1, r4, r5, r6}
	pop	{r0}
	bx	r0
.L3:
	movs	r2, #128
	movs	r1, #0
	lsls	r2, r2, #2
	b	.L6
.L8:
	.align	2
.L7:
	.word	SetBgPosition
	.word	SetBgMapDataOffset
	.word	SetBgTileDataOffset
	.word	gLCDIOBuffer
	.word	16780288
	.word	gBg0MapBuffer
	.word	CpuSet
	.word	16777224
	.word	CpuFastSet
	.word	100679680
	.word	Decompress
	.word	gBg3MapBuffer
	.word	CopyToPaletteBuffer
	.word	EnableBgSyncByMask
	.word	EnablePaletteSync
	.size	CGC_LoadMultiPalBG, .-CGC_LoadMultiPalBG
	.ident	"GCC: (devkitARM release 55) 10.2.0"
	.code 16
	.align	1
.L10:
	bx	r3
.L9:
	bx	r6
