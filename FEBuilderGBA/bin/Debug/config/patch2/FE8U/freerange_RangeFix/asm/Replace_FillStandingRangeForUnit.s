.thumb
.include "_Definitions.h.s"

.set prFillRangeByMask, EALiterals+0x00

@ .org 0x01B460
@ Arguments: r0 = Unit Struct, r1 = Range Mask
Replace_FillStandingRangeForUnit:
	push {lr}
	
	mov r2, r1
	
	ldrb r1, [r0, #0x11]
	ldrb r0, [r0, #0x10]
	
	ldr r3, prFillRangeByMask
	_blr r3
	
	pop {r0}
	bx r0
	
.ltorg
.align

EALiterals:
	@ POIN prFillRangeByMask
