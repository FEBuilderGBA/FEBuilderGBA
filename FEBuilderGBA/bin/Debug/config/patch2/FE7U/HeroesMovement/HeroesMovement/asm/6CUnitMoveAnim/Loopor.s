.thumb
.include "../../../_StanHaxCommon/asm/_Definitions.h.s"

.set prMoveMOVEUNITTowards, EALiterals+0x00
.set ANIM_MOVE_SPEED,       EALiterals+0x04

UnitMoveAnim_Loopor:
	push {r4, lr}
	
	mov r4, r0
	
	@ Prepare routine call
	ldr r3, prMoveMOVEUNITTowards
	mov lr, r3
	
	@ r3 = Speed
	ldr r3, ANIM_MOVE_SPEED
	
	@ Loading fast anim option value
	ldr  r0, =(pChapterDataStruct + 0x40)
	ldrb r0, [r0]
	lsr r0, r0, #7 @ r0 = 1 if fast anim is set, 0 otherwise
	
	@ add 1 to speed if fast speed is set
	add r3, r0
	
	@ Loading Unit Struct
	ldr r2, [r4, #0x30]
		@ Getting Target Position
		ldrb r1, [r2, #0x10]
		ldrb r2, [r2, #0x11]
	
	@ Loading MOVEUNIT into r0
	ldr r0, [r4, #0x2C]
	
	@ MOVE
	.short 0xF800
	
	@ Checking return value (0 means didn't move)
	cmp r0, #0
	bne End
	
	@ Breaking 6C Loop (which effectively means we're ending the animation)
	mov r0, r4
	_blh pr6C_BreakLoop
	
End:
	pop {r4}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMoveMOVEUNITTowards
	@ WORD ANIM_MOVE_SPEED
