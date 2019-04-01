.include "../../_StanHaxCommon/asm/_Definitions.h.s"

push {lr}

	mov  r2, #0x44
	ldrb r1, [r0,r2]
	cmp  r1, #0x3        @AI2 03=移動しない
	bne  TureReturn

	mov r0,#0x00
	b   Exit

TureReturn:
	mov r0,#0x01

Exit:

pop {r1}
bx r1

