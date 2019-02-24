.thumb
.include "../../../_CommonASM/_Definitions.h.s"

ShoveAnimWait6C_Destruct:
	push {lr}

	@ All we are doing here is setting the Target Standing Sprite Display back on
	@ ---------------------------------------------------------------------------
	
	@ Loading Target Unit Struct
	ldr r0, [r0, #0x38]
	
	@ Clearing bit 1 in Unit State bitfield
	ldr r1, [r0, #0xC]
		mov r2, #0x01
		bic r1, r2 @ r1 = r1 & ~r2
	str r1, [r0, #0xC]
	
	@ End
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ nothing
