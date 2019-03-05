.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set HeroesMovement_Table, EALiterals+0x00

push {r4,r5,lr}
	mov r4, r0  @CurrentUnit
	mov r5, r1  @SkillID

	ldr r3, HeroesMovement_Table
	sub r3, #0x4
Loop:
	add r3, #0x4

CheckSkillID:
	ldrb r0, [r3,#0x03]
	cmp r0, #0x00
	beq NotFound;

	cmp r0, r5
	bne Loop

CheckUnitID:
	ldrb r0, [r3,#0x00]
	cmp r0, #0x00
	beq CheckClassID

	ldr r1, [r4,#0x00]
	ldrb r1, [r1,#0x04]
	cmp r0, r1           @unitram->unit->id
	bne Loop

CheckClassID:
	ldrb r0, [r3,#0x01]
	cmp r0, #0x00
	beq CheckItemID

	ldr r1, [r4,#0x04]
	ldrb r1, [r1,#0x04]
	cmp r0, r1           @unitram->class->id
	bne Loop

CheckItemID:
	ldrb r0, [r3,#0x02]
	cmp r0, #0x00
	beq Found

	ldrb r1, [r4, #0x1e]    @unitram->item1
	cmp  r0, r1
	beq  Found

	mov  r1, #0x20
	ldrb r1, [r4, r1]    @unitram->item2
	cmp  r0, r1
	beq  Found

	mov  r1, #0x22
	ldrb r1, [r4, r1]    @unitram->item3
	cmp  r0, r1
	beq  Found

	mov  r1, #0x24
	ldrb r1, [r4, r1]    @unitram->item4
	cmp  r0, r1
	beq  Found

	mov  r1, #0x26
	ldrb r1, [r4, r1]    @unitram->item5
	cmp  r0, r1
	bne  Loop

Found:
	mov r0,#0x01
	b Exit

NotFound:
	mov r0,#0x00

Exit:

pop {r4,r5}
pop {r1}
bx r1

.ltorg
.align
EALiterals:
	@ POIN HeroesMovement_Table
