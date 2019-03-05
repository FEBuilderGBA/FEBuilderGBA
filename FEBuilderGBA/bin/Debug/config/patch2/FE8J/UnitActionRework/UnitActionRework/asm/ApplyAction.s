.thumb
.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set pActionEffectTable, EALiterals+0x00

.set prActionStaffDoorChestUseItemNightmare, 0x0802FC48

ApplyAction:
	push {r4-r5, lr}
	
	@ r4 = Whatever 6C calls this (from vanilla: either E_PLAYERPHASE (Player Phase Manager) or E_CPPERFORM (AI Execution))
	mov r4, r0
	
	@ r5 = Action Struct
	ldr r5, =pActionStruct
	
	ldrb r0, [r5, #0x0C] @ Action Unit Index
	_blh prUnit_GetStruct
	
	@ Storing Active Unit
	ldr r1, =ppActiveUnit
	str r0, [r1]
	
	@ r2 = Action Id
	ldrb r2, [r5, #0x11]
	
	cmp r2, #2 @ ATTACK
	bne SkipShittyHardcodedNightmareCheck
	
	@ r1 = Action Item Index
	ldrb r1, [r5, #0x12]
	
	lsl r1, #1    @ r1 = item index * 2
	add r1, #0x1E @ r1 = 0x1E + item index * 2
	add r1, r0    @ r1 = Unit.itemShorts[item index]
	
	ldrh r0, [r1]
	
	mov r1, #0xFF
	and r0, r1 @ Getting the Index Id from the Item Short
	
	cmp r0, #0xA6 @ Freaking Nightmare, of course
	bne SkipShittyHardcodedNightmareCheck
	
	@ NIGHTMARE SPECIAL CASE BEGIN
	
	mov r0, r4
	_blh prActionStaffDoorChestUseItemNightmare
	
	mov r0, #0
	
	b End
	
	@ NIGHTMARE SPECIAL CASE END

.ltorg
	
SkipShittyHardcodedNightmareCheck:
	ldr r3, pActionEffectTable
	lsl r0, r2, #2 @ r0 = (Action Id * 4)
	add r0, r3     @ r0 = Offset of table index
	
	ldr r0, [r0]
	
	@ r5 is now the top 4 bits of the entry
	lsr r5, r0, #28
	
	@ Clearing the top 4 bits of the entry
	lsl r2, r0, #4
	lsr r2, #4 @ Remember? Sets Z flag
	
	beq Continue @ Continue when routine is null
	
	@ Calling routine
	mov r0, r4
	bl BXR2
	
	@ Goto end if Forced Yield not set
	cmp r5, #0
	beq End
	
	mov r0, #0 @ Yield (6C)
	
	b End
	
Continue:
	mov r0, #1 @ Continue (6C)
	
End:
	pop {r4-r5}
	
	pop {r1}
	bx r1

BXR2:
	bx r2

.ltorg
.align

EALiterals:
	@ POIN Action Effect Table
