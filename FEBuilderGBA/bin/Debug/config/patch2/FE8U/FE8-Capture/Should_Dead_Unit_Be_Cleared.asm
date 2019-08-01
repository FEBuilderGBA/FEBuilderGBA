@marge boviex's fix
@
@https://github.com/FireEmblemUniverse/SkillSystem_FE8/commit/920ac7a8001a28591b32df6f9c12b0b46ab74e3c
@
.thumb

@r2 = char data of dead unit
@return 1 in r1 if unit should be cleared, or 0 if they should not

ldrb	r1,[r2,#0xB]
mov		r0,#0xC0
and		r1,r0
cmp		r1,#0
beq		GoBack		@player units don't get cleared
ldr		r0,[r2,#0xC]
mov		r3,#0x20	@being rescued
tst		r0,r3
beq		GoBack
mov		r1,#0
GoBack:
ldr		r0,=#0x8018409
bx		r0

.ltorg