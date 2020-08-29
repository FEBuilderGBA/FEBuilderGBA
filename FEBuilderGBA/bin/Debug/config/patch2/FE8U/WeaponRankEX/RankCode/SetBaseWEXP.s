.thumb



push {r0,r1,r2,r3}

ldr r3, =StartWEXP
ldr r3, [r3, #0x0]
ldrb r0, [r3, #0x0]
cmp r0, #0x1
beq BasesSet
	mov r1, #0x1
	strb r1, [r3]
	ldr r2,=ArrayLoopCount
	ldrh r2, [r2, #0x0]
	mov r1, #0x0
	mov r0, #0x0
	add r3, r3, #0xf
		ArrayLoop:
		add r0, r0, #0x1
		add r3, r3, #0x1
		strb r1, [r3]
		sub r2, r2, #0x1
		cmp r2, #0x0
		beq BasesSet
		cmp r0, #0x8
		bge SSSkip
		b ArrayLoop
	SSSkip:
	add r3, r3, r0
	mov r0, #0x0
	b ArrayLoop
BasesSet:
pop {r0,r1,r2,r3}
bx lr
		
		