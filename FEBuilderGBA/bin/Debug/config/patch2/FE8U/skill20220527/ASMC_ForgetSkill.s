
	.thumb

	gBWLTable  = 0x0203E884 @ Where the skills are stored
	gEventSlot = 0x030004B8 @ Where the event slot values are stored

	GetUnitFromEventParam = 0x0800BC50|1 @ Given an event parameter (either a char id or -1/-2/-3 special values), gets a Unit*

ASMC_ForgetSkill:
	push {r4-r6, lr}

	@ PUT SKILL ID IN SLOT 1
	@ PUT CHAR ID IN SLOT 2

	@ Sets Slot C to 1 if skill was successfully forgotten, 0 otherwise

	@ r0 = char id
	ldr r1, =gEventSlot
	ldr r0, [r1, #(0x02 * 4)] @ Load unit from event slot 2

	ldr r3, =GetUnitFromEventParam
	bl BXR3

	ldr  r0, [r0] @ CharData
	ldrb r0, [r0, #4] @ CharId

	@ r1 = skill id
	ldr r1, =gEventSlot
	ldr r1, [r1, #(0x01 * 4)] @ Load skill from event slot 1

	@ r3 = Learned Skill List for Active Char
	ldr r3, =gBWLTable
	lsl r0, #4 @ CharId * sizeof(BWLEntry)
	add r3, r0
	add r3, #1 @ Learned Skills start at byte 1 in BWL Entry

	@ r2 = current slot
	mov r2, #0

loop_find_slot:
	@ r0 = learned skill in slot #r2
	ldrb r0, [r3, r2]

	@ compare skill
	cmp r0, r1
	beq found_slot

	@ add current slot (check next slot next iteration)
	add r2, #1

	@ loop back if we haven't done all skill slots yet
	cmp r2, #4
	blt loop_find_slot

	@ we reached end of learned skill list
	@ we didn't find the skill we want to forget
	@ so we end, bye

	@ set slot C to 0/false
	ldr r1, =gEventSlot
	mov r0, #0
	str r0, [r1, #(0x0C * 4)]

	b end

found_slot:
loop_remove:
	add r0, r2, #1 @ r0 = next slot id

	ldrb r0, [r3, r0] @ load skill after current slot
	strb r0, [r3, r2] @ store into current slot

	add r2, #1

	cmp r2, #4
	blt loop_remove

	@ set last skill as zero
	@ otherwise it would have been whatever is after the skill list in memory
	@ aka unwanted garbage
	mov  r0, #0
	strb r0, [r3, #3]

	@ set slot C to 1/true
	ldr r1, =gEventSlot
	mov r0, #1
	str r0, [r1, #(0x0C * 4)]

end:
	pop {r4-r6}

	pop {r3}
BXR3:
	bx r3
