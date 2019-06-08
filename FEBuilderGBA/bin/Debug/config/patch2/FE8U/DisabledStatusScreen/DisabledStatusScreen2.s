@Hook 879C4

.thumb

@r2 class
@r3 keep

push {r4}

ldrb r0, [r2, #0x4]
ldr r4, Table

Loop:
	ldrb r1,[r4]
	cmp r1,#0x00
	beq Display_Exit

	cmp r0,r1
	beq Found

	add r4,#0x01
	b   Loop

Found:
pop {r4}
ldr r0, =0x8087930+1
bx  r0

Display_Exit:
pop {r4}
ldr r0, =0x80879CE+1
bx  r0

.align 4
.ltorg
Table:
