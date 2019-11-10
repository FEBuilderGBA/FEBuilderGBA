.thumb
mov r4 ,r0
ldrb r1, [r2, #0xF] @ramunit->status4
lsr  r1,#0x7
cmp r1,#0x01
