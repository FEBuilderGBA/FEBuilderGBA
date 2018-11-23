.thumb

ldr r0, =#0x0203A3F0
ldr r0, [r0]
cmp r0, #0x00000000
beq or1

and1:
ldr r0, =0x0202BC07
ldrb r0, [r0, #0x00]
cmp r0, #0x00
bne ReturnSequence

ldr r0, GetRandomNumberFunction
bl bxr0
b ReturnSequence

or1:
ldr r0, =#0x030046AA 
ldrb r0, [r0]
cmp r0, #0x00
bne and1

ReturnSequence:
@overwritten code
ldr r0, =0x2024C70
ldr r1, [r0]
cmp r1, #0x0
beq bx19E8

ldr r0, RandomizerReturnTo
bx r0

bxr0:
bx r0

bx19E8:
ldr r0, =0x080019E8
bx r0

.align 4
RandomizerReturnTo:
.long 0x080019DC|1
GetRandomNumberFunction:
.long 0x08000E04|1
