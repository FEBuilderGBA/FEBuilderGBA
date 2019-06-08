.thumb

@hooks at 80816da
mov r0, r6
add r0, #0x50 @buffer ptr?
ldr r0, [r0]
sub r0, #8 @prev round
mov r1, #5
ldsb r1, [r0, r1] @amount to change hp
neg r1, r1
mov r0, r10
ldr r3, =0x80816e7
bx r3
