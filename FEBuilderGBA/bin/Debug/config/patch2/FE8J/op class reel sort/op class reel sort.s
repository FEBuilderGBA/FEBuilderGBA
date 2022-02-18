.thumb

@ORG 0xB8C80	@{J}
ldr r3, =0x08AB0B18	@class reel pointer	@{J}
ldr r3, [r3]
mov r2, #28
mul r1, r2
add r0, r3, r1

ldrb r1, [r0, #0xF]
cmp r1, #0x4
ble Exit

mov r0, #0x0
Exit:
bx lr
