.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

@ Arguments: r0 = Subject Unit Struct, r1 = Target X, r2 = Target Y
@ Returns: r0 = Pos Pair (additionally: r1 = Pos X, r2 = Pos Y)
GetOppositePosition:
	ldrb r3, [r0, #0x10]
		sub r1, r3 @ r1 = direction.x = target.x - unit.x
		lsl r1, #1 @ r1 = direction.x*2
		add r1, r3 @ r1 = unit.x + direction.x*2
	
	ldrb r3, [r0, #0x11]
		sub r2, r3 @ r2 = direction.y = target.y - unit.y
		lsl r2, #1 @ r2 = direction.y*2
		add r2, r3 @ r2 = unit.y + direction.y*2
	
	_MakePair r0, r1, r2
	
	bx lr

.ltorg
.align

EALiterals:
	@ nothing here sir
