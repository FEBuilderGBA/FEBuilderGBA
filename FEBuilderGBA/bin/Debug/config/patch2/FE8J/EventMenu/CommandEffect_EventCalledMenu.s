.thumb

@ _blh macro, for absolute branch with link
.macro _blh to, reg=r3
	ldr \reg, =\to
	mov lr, \reg
	.short 0xF800
.endm

CommandEffect_EventCalledMenu:
	push {lr}
	
	@ r1 is command 6C
	@ command 6C + 0x3C is command index
	
	mov  r0, #0x3C
	ldrb r0, [r1, r0]
	
	_blh #0x0800d4bc @ SetEventSlotC
	
	mov r0, #0x17
	
	pop {r1}
	bx r1
