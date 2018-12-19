.thumb
.org 0

push {lr}

	ldr  r0,=#0x03004DF0      @操作中のユニット {J}
@	ldr  r0,=#0x03004E50      @操作中のユニット {U}

ldr  r0,[r0]
cmp  r0, #0x00
beq  NotTheif

ldr  r0,[r0,#0x04]        @RamUnit->Class
ldrb r0,[r0,#0x04]        @ Get ROM Class Struct

ldr r3, MultiDiscoverDesertTreasuresTable

Loop:
ldrb r1, [r3]
cmp r1, #0x00
beq NotTheif
cmp r0, r1
beq Theif

add r3, #0x1
b   Loop

Theif:
mov r0,#0x01
b Exit

NotTheif:
mov r0,#0x00
@b Exit

Exit:
	ldr	r2, =0x030004E0       @FE8J SlotC
@	ldr	r2, =0x030004E8       @FE8U SlotC
	str	r0, [r2, #0x0]

pop	{r1}
bx	r1

.ltorg
MultiDiscoverDesertTreasuresTable:
@list of the Data List sizeof 1bytes  0x00==TERM
@struct
@byte classid
