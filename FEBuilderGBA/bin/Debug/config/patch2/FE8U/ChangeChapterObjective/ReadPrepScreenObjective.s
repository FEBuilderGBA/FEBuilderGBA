.thumb
@ORG 0x95A90	@FE8U

.set GetChapterObjective, EALiterals+0x00

.macro blr reg
	mov lr, \reg
	.short 0xF800
.endm

mov		r0, #0x0 @short chapter objective
ldr		r3, GetChapterObjective @and this gives us the textid in r0
blr		r3

ldr		r3, =0x08095AB0+1	@FE8U
@ldr	r3, =0x08097D90+1	@FE8J
bx		r3

.ltorg
.align

EALiterals:
	@ POIN GetChapterObjective
