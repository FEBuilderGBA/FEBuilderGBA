.include "../../_StanHaxCommon/asm/_Definitions.h.s"

.set prSillTestr, EALiterals+0x00
.set SkillID, EALiterals+0x04

push {lr}

	ldrb r1, [r0,#0x0c]
	mov  r2, #0x40
	and  r1, r2          @US_CANTOING
	cmp  r1, #0x00
	bne  FalseReturn     @再移動中なら不可

	ldr r1, SkillID

	ldr r3, prSillTestr
	_blr r3

	cmp r0,#0x01
	bne FalseReturn

	mov r0,#0x01
	b   Exit

FalseReturn:
	mov r0,#0x00

Exit:

pop {r1}
bx r1

.ltorg
.align
EALiterals:
	@ POIN prSillTestr
	@ WORD SkillID
