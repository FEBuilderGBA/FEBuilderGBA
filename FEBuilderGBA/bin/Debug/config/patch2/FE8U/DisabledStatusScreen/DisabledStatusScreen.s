@Hook 1C92C

.thumb

@r0 class id

ldrb r0, [r0, #0x4]
ldr r3, Table

Loop:
	ldrb r1,[r3]
	cmp r1,#0x00
	beq Display_Exit

	cmp r0,r1
	beq Found

	add r3,#0x01
b   Loop

Found:
mov r0, #0x0
b   Exit

Display_Exit:
mov r0 , #0x1

Exit:
pop {r1}
bx r1

.align 4
.ltorg
Table:
