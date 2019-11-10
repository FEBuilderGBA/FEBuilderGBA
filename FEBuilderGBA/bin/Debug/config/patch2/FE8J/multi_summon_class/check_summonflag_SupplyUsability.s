.thumb
ldrb r0, [r0, #0xF] @ramunit->status4
lsr  r0,#0x7
cmp r0,#0x01
