.thumb
ldrb r0, [r2, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd141	@bne #0x80252c4
ldr r3, [r4, #0x4]
ldrb r0, [r4, #0xF] @ramunit->status4
lsr  r0,#0x7
.short 0xd13d	@bne #0x80252c4
nop
