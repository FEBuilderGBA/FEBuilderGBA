.thumb
.include "../../../_StanHaxCommon/asm/_Definitions.h.s"

.set p6CCode, EALiterals+0x00

@ Arguments: r0 = Subject Unit Struct, r1 = Facing Direction
@ Returns: r0 = New 6C
UnitMoveAnim_Constructor:
	push {r4-r6, lr}
	
	@ Saving arguments for later use
	mov r4, r0 @ r4 = Unit Struct
	mov r5, r1 @ r5 = Facing Direction
	
	@ Setting up New6C Arguments
	ldr r0, p6CCode @ r0 = 6C code pointer
	mov r1, #3      @ r1 = Main 3 6C Thread
	
	@ Making 6C
	_blh pr6C_New
	mov r6, r0 @ r6 = New 6C
	
	@ Field 0x30 = Subject Unit Struct
	str r4, [r6, #0x30]
	
	@ Making Subject MOVEUNIT
	mov r0, r4
	_blh prMOVEUNIT_NewForMapUnit
	
	@ Field 0x2C = Subject MOVEUNIT
	str r0, [r6, #0x2C]
	
	@ Facing Direction
	mov r1, r5
	_blh prMOVEUNIT_SetSprDirection

	@ Hiding Target Unit Standing Sprite (by setting bit 1 in State bitfield)
	ldr r0, [r4, #0xC]
		mov r2, #1
		orr r0, r2
	str r0, [r4, #0xC]
	
	@ Returning new 6C
	mov r0, r6
	
	@ End
	pop {r4-r6}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN p6CCode
