.thumb
ldrb r0, [r2, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd120	@bne #0x80253aa
ldrb r0, [r4, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd11d	@bne #0x80253aa
nop
nop
