.thumb
.include "../../../_StanHaxCommon/asm/_Definitions.h.s"

.set prMoveMOVEUNITTowards, EALiterals+0x00
.set PUSH_MOVE_SPEED,       EALiterals+0x04

UnitPushAnim_PushLoopor:
	push {r4-r5, lr}
	
	mov r4, r0
	
	@ Prepare routine call
	ldr r3, prMoveMOVEUNITTowards
	mov lr, r3
	
	@ r3 = Speed
	ldr r3, PUSH_MOVE_SPEED
	
	@ Loading fast anim option value
	ldr  r0, =(pChapterDataStruct + 0x40)
	ldrb r0, [r0]
	lsr r0, r0, #7 @ r0 = 1 if fast anim is set, 0 otherwise
	
	@ add 1 to speed if fast speed is set
	add r3, r0

	@ Getting Target X
	mov  r1, #0x34
	ldrh r1, [r4, r1]
	
	@ Getting Target Y
	mov  r2, #0x36
	ldrh r2, [r4, r2]

	@ Loading MOVEUNIT into r0
	ldr r0, [r4, #0x2C]
	
	@ MOVE
	.short 0xF800
	
	ldr r0, [r4, #0x38]
	sub r0, #1
	str r0, [r4, #0x38]
	
	cmp r0, #0
	bgt End @ If countdown didn't reach zero yet, goto end
	
	@ Break loop
	mov r0, r4
	_blh pr6C_BreakLoop
	
End:
	pop {r4-r5}
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prMoveMOVEUNITTowards
	@ WORD PUSH_MOVE_SPEED
