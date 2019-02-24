.thumb
.include "../../../_CommonASM/_Definitions.h.s"
.include "../_Config.h.s"

.set pShoveAnimWait6CCode, EALiterals+0x00

@ Arguments: r0 = Subject Unit Struct, r1 = Target Unit Struct
@ Returns: r0 = New 6C
ShoveAnimWait6C_New:
	push {r4-r7, lr}
	
	@ We will need r8 here, so pushing r8 too
	mov r4, r8
	push {r4}
	
	@ Saving arguments for later use
	mov r4, r0
	mov r5, r1
	
	@ Setting up New6C Arguments
	ldr r0, pShoveAnimWait6CCode @ r0 = 6C code pointer
	mov r1, #3                   @ r1 = Main 3 6C Thread
	
	@ Making 6C
	_blh pr6C_New
	mov r6, r0
	
	@ Field 0x34 = Subject Unit Struct
	str r4, [r6, #0x34]
	
	@ Field 0x38 = Target Unit Struct
	str r5, [r6, #0x38]
	
	.ifdef SPINNING_SHOVE
		mov r0, #0
		mov r1, #0x2A
		strb r0, [r6, r1]
	.endif
	
	@ Making Subject MOVEUNIT
	mov r0, r4
	_blh prMOVEUNIT_NewForMapUnit
	
	@ Field 0x2C = Subject MOVEUNIT
	str r0, [r6, #0x2C]
	
	@ Moving MOVEUNIT into r8 for later use
	mov r8, r0
	
	@ Loading Subject Position
	ldrb r0, [r4, #0x10]
	ldrb r1, [r4, #0x11]
	
	@ Loading Target Position
	ldrb r2, [r5, #0x10]
	ldrb r3, [r5, #0x11]
	
	@ Getting Facing Index
	_blh prGetFacingDirectionId, r7
	
	@ Setting Facing for MOVEUNIT
	mov r1, r0
	mov r0, r8
	
	_blh prMOVEUNIT_SetSprDirection, r7
	
	@ Doing same thing for target MOVEUNIT
	
	mov r0, r5
	_blh prMOVEUNIT_NewForMapUnit
	
	str r0, [r6, #0x30]
	mov r8, r0
	
	ldrb r0, [r5, #0x10]
	ldrb r1, [r5, #0x11]
	
	ldrb r2, [r4, #0x10]
	ldrb r3, [r4, #0x11]
	
	_blh prGetFacingDirectionId, r7
	
	mov r1, r0
	mov r0, r8
	
	_blh prMOVEUNIT_SetSprDirection, r7
	
	@ Hiding Target Unit Standing Sprite (by setting bit 1 in State bitfield)
	ldr r1, [r0, #0xC]
		mov r2, #1
		orr r1, r2
	str r1, [r0, #0xC]
	
	@ Returning new 6C
	mov r0, r6
	
	@ Popping r8
	pop {r4}
	mov r8, r4
	
	@ End
	
	pop {r4-r7}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN pShoveAnimWait6CCode
