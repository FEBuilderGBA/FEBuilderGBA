.thumb

@overwritten code
push {r7,r14}
add sp,#0x-4
mov r7,r13
str r0,[r7]

ldr r3,=#0x0203A3F0
ldr r0,=#0x00000000
str r0,[r3]

ldr r3,=#0x030046AA
ldrb r0,=#0x00
str r0,[r3]

ldr r3,=#0x08000D7A|1
bx r3
