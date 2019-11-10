.thumb
ldrb r0, [r2, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd120	@bne #0x802535a
ldrb r0, [r4, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd11d	@bne #0x802535a
nop
nop
