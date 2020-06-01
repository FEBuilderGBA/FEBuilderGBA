.thumb

push    {r4,r14}
ldr     r2,setval_args
ldr     r0,easy_mode
ldr     r4,[r2,#0x4]
mov     r1,#0x20
ldrb    r2,[r0]
cmp     r4,#0x0
ble     set_easy_mode
orr     r2,r1
strb    r2,[r0]
cmp     r4,#0x1
bne     set_hard_mode
not_hard:
mov     r1,#0x40
ldr     r2,hard_mode
ldrb    r3,[r2]
bic     r3,r1
b       not_easy
set_easy_mode:
bic     r2,r1
strb    r2,[r0]
b       not_hard
set_hard_mode:
ldr     r2,hard_mode
mov     r3,#0x40
ldrb    r1,[r2]
orr     r3,r1
not_easy:
strb    r3,[r2]
pop     {r4}
pop     {r0}
bx      r0

setval_args:
.long	0x030004B8
easy_mode:
.long	0x0202BD32
hard_mode:
.long	0x0202BD04
