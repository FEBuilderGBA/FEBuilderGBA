.thumb

@r5 = char data

ldr		r0,[r5,#0xC]
ldr		r1,=#0x30004B8
ldr		r1,[r1,#0x4]
bic		r0,r1
str		r0,[r5,#0xC]
ldr		r0,=#0x801049B
bx		r0

.ltorg
