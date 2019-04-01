.thumb
@ORG 0x8E538

.set GetChapterObjective, EALiterals+0x00

.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

mov		r0, #0x1 @long chapter objective
ldr		r3, GetChapterObjective @and this gives us the textid in r0
blr		r3

@ldr		r3, =0x0808E54C+1	@FE8U
ldr	r3, =0x08090820+1	@FE8J
bx		r3

.ltorg
.align

EALiterals:
	@ POIN GetChapterObjective
