@Call 7FF8C {J}

.thumb

ldr r1, [r5, #0x30]
cmp r1, #0x0
bge Skip

mov r1, #0x0

Skip:
add r1, #0x4

ldr r2, [r5, #0x34]
ldr r3, =0x000041C0

str r4,[sp, #0x0]

ldr r4, =0x0807FF96|1	@{J}
bx r4
