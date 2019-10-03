pointerList = 0x817C00
.thumb

.org 0x60748
	push {r4,r7,lr}
	add sp, #-0x14
	mov r7, sp
	str r0, [r7, #0x0]
	ldr r1, [r7, #0x0]
	add r0, r1, #0x0
	add r1, #0x3b
	ldrb r2, [r1, #0x0]				@loads class ID
	ldr r1, =pointerList+0x8000000	@at offset 0607C0
	lsl r2, r2, #2
	ldr r0, [r1, r2]
	str r0, [r7, #8]

#
# r7   some data
# r7+4 class data pointer (not used)
# r7+8 walking sound data
#

doSound:
	ldr r0, [r7, #0x0]
	add r1, r0, #0x0
	add r0, #0x3d
	ldrb r1, [r0, #0x0]
	add r2, r1, #0x1
	add r3, r2, #0x0
	strb r3, [r0, #0x0]
	lsl r1, r1, #0x18
	lsr r0, r1, #0x18
	ldr r1, [r7, #0x8]
	ldrh r2, [r1, #0x0]
	add r1, r2, #0x0
	bl 0x9dce4

	str r0, [r7, #0xc]
	add r1, r7, #0x0
	add r1, #0x10
	ldr r0, [r7, #0x0]
	bl 0x60b68

	ldr r0, [r7, #0xc]
	add r1, r0, #0x0
	lsl r0, r1, #0x1
	ldr r1, [r7, #0x8]
	add r0, r0, r1
	add r1, r0, #0x4
	ldrh r0, [r1, #0x0]
	cmp r0, #0x0
	beq 0x60836

	ldr r0, [r7, #0xc]
	add r1, r0, #0x0
	lsl r0, r1, #0x1
	ldr r1, [r7, #0x8]
	add r0, r0, r1
	add r1, r0, #0x4
	ldrh r0, [r1, #0x0]
	ldr r1, [r7, #0x8]
	add r2, r1, #0x2
	ldrh r1, [r2, #0x0]
	add r3, r7, #0x0
	add r3, #0x10
	mov r4, #0x0
	ldsh r2, [r3, r4]
	bl 0x5fec8

	add sp, #0x14
	pop {r4,r7,pc}

.pool

.org pointerList
.incbin "pointerList.bin"
