.thumb
.include "_Definitions.h.s"

.set Replace_FillReachRangeForUnit_Fast, EALiterals+0x00
.set Replace_FillReachRangeForUnit_FastWithBallista, EALiterals+0x04
.set Replace_FillReachRangeForUnit_FreeRangeNormal, EALiterals+0x08
.set Replace_FillReachRangeForUnit_FreeRangeWithBallista, EALiterals+0x0C

@ .org 0x01A998
Replace_FillReachRangeForUnit:
push {r4,r5,r6,r7,lr}
mov r7, r11
mov r6, r10
mov r5, r9
mov r4, r8
push {r4,r5,r6,r7}

	@r0 = RAM Unit

CheckBallista:
	@ (r0, r1) = (char, class)
	ldr r1, [r0]
	ldr r2, [r0, #4]
	
	@ (r0, r1) = (char.attr, class.attr)
	ldr r1, [r1, #0x28]
	ldr r2, [r2, #0x28]

	@ r0 = Unit Attributes
	orr r1, r2

	mov r4, #0x80 @ Ballista Flag
	and r4, r1

	@r0 = RAM Unit
	
	mov r1, #1
	neg r1, r1
	_blh GetUnitRangeMask

Check11: @sword
	cmp r0, #0x2
	bne Check12
	mov r1, #0x1
	mov r2, #0x0
	b   Fast
Check12: @magic
	cmp r0, #0x6
	bne Check13
	mov r1, #0x2
	mov r2, #0x0
	b   Fast
Check13: @
	cmp r0, #0xE
	bne Check22
	mov r1, #0x3
	mov r2, #0x0
	b   Fast
Check22: @bow
	cmp r0, #0x4
	bne Check23
	mov r1, #0x2
	mov r2, #0x1
	b   Fast
Check23: @long-bow
	cmp r0, #0xC
	bne Check24
	mov r1, #0x3
	mov r2, #0x1
	b   Fast
Check24:
	cmp r0, #0x1C
	bne Check14
	mov r1, #0x4
	mov r2, #0x1
	b   Fast
Check14:
	cmp r0, #0x1E
	bne Check00
	mov r1, #0x4
	mov r2, #0x0
	b   Fast
Check00:
	cmp r0, #0x01
	bgt FreeRange

	cmp r4, #0x0
	bne FreeRange_Ballista
	b   Exit

FreeRange:
	cmp r4, #0x0
	bne FreeRange_Ballista
	ldr r3, Replace_FillReachRangeForUnit_FreeRangeNormal
	bx  r3

FreeRange_Ballista:
	ldr r3, Replace_FillReachRangeForUnit_FreeRangeWithBallista
	bx  r3

Fast:
	cmp r4, #0x0
	bne Fast_Ballista
	ldr r3, Replace_FillReachRangeForUnit_Fast
	bx  r3

Fast_Ballista:
	ldr r3, Replace_FillReachRangeForUnit_FastWithBallista
	bx  r3

Exit:
pop {r0,r1,r2,r3,r4,r5,r6,r7}
mov r8, r0
mov r9, r1
mov r10, r2
mov r11, r3
pop {r3}
bx r3

.ltorg
.align
EALiterals:
