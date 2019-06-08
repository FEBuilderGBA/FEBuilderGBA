@Hook 874AC

.thumb

@r0 class
@r1 !keep
@r2 !keep
@r3 temp
@r7 temp

ldrb r3, [r0, #0x4]
ldr r7, Table

Loop:
	ldrb r0,[r7]
	cmp r0,#0x00
	beq Display_Exit

	cmp r0,r3
	beq Found

	add r7,#0x01
	b   Loop

Found:
ldr r0, =0x08087532 + 1
bx  r0

Display_Exit:
ldr r0, =0x080874B6 + 1
bx  r0

.align 4
.ltorg
Table:
