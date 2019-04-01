.include "../../_StanHaxCommon/asm/_Definitions.h.s"

push {lr}

	ldrb r1, [r0,#0xB]
	@cmp  r1, #0x80       @敵軍だったらダメ
	cmp  r1, #0x40      @友軍もダメにした場合は、こっちら
	bge  FalseReturn

	mov r0,#0x01
	b   Exit

FalseReturn:
	mov r0,#0x00

Exit:

pop {r1}
bx r1

