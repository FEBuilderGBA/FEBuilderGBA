.thumb

@ORG 0xB40EC	@{U}
ldr r3, =0x08a2fcb4	@class reel pointer	@{U}
ldr r3, [r3]
mov r2, #20
mul r1, r2
add r0, r3, r1

ldrb r1, [r0, #0x6]
cmp r1, #0x4
ble Exit

mov r0, #0x0
Exit:
bx lr
