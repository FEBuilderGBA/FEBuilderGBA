.thumb

push {r1}
push {r0}
mov r7, #0x2
ldr r0,=lMaxRank
ldrb r0, [r0]
cmp r5, r0
blt Jump877
	mov r7, #0x4
Jump877:
add r4, #0x4
pop {r1}
pop {r0}
ldr r3,=0x080877c7
bx r3