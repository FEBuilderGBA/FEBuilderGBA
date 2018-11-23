.thumb

@overwritten code
add sp,#-0x4
mov r7,r13
mov r0,#0x0
str r0,[r7]

ldr r3,=#0x0203A3F0
ldr r0,=#0xFFFFFFFF
str r0,[r3]

ldr r3,=#0x030046AA
ldrb r0,=#0xFF
str r0,[r3]

pop {r3}
ldr r0,=#0x08000B62|1
bx r0
