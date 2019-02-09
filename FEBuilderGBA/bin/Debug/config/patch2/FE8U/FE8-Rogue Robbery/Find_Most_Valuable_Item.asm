.thumb

.equ origin, 0x3DC0C
.equ NextUnit, . + 0x3DC56 - origin
.equ ChangeValue, . + 0x3DC30 - origin

@r0=char data ptr

ldr		r3,Most_Valuable_Item
mov		r14,r3
.short	0xF800
mov		r2,#1
cmn		r1,r2
beq		NextUnit
lsl		r6,r1,#0x18		@r6 = inventory slot of most expensive item
ldr		r3,[sp,#0x14]	@worth of current most expensive item
cmp		r0,r3
blo		NextUnit
b		ChangeValue

.align
Most_Valuable_Item:
@
