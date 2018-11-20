.thumb
.org 0x0

@r0=defender's weapon's ability word, r4=attacker, r5=defender+0x48. Paste at 28AA6
mov		r2,#0x17	@default: def
mov		r1,#0x2
tst		r0,r1
beq		IsStr
mov		r2,#0x18
IsStr:
mov		r1,#0x40
tst		r0,r1
beq		NoSwitch
mov		r3,#0x18
cmp		r2,r3
beq		Switch
mov		r2,r3
b		NoSwitch
Switch:
mov		r2,#0x17
NoSwitch:
mov		r0,#0x56
cmp		r2,#0x17
beq		UseTerrainDef
mov		r0,#0x58
UseTerrainDef:
ldsb	r0,[r4,r0]	@attacker's terrain def/res value
ldsb	r2,[r4,r2]
add		r0,r0,r2
add		r4,#0x5C	@defense halfword
strh	r0,[r4]
pop		{r4-r5}
pop		{r0}
bx		r0
