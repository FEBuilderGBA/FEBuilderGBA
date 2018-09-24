.thumb
.include "_Definitions.h.s"

.set EAL_prSkillTester, (EALiterals+0x00)

.set prGetUnitFromEventParam, (0x0800BC50+1)

ASMC:
	push {lr}
	
	ldr r0, =pEventSlot0
	ldr r0, [r0, #(0x02 * 4)] @ Load unit from event slot 2
	
	_blh prGetUnitFromEventParam
	
	ldr r1, =pEventSlot0
	ldr r1, [r1, #(0x01 * 4)] @ Load skill from event slot 1
	
	ldr r3, EAL_prSkillTester
	mov lr, r3
	.short 0xF800
	
	ldr r1, =pEventSlot0
	str r0, [r1, #(0x0C * 4)] @ Store result in slot C
	
	pop {r1}
	bx r1

.ltorg
.align

EALiterals:
	@ POIN prSkillTester
