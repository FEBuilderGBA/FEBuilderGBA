.thumb
@ORG 0x8D2D8

.set GetChapterObjective, EALiterals+0x00

.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

mov		r0, #0x0 @short chapter objective
ldr		r3, GetChapterObjective @and this gives us the textid in r0
blr		r3

ldr		r3, =0x0808d2ec+1	@FE8U
@ldr	r3, =0x0808F5E8+1	@FE8J
bx		r3

.ltorg
.align

EALiterals:
	@ POIN GetChapterObjective
