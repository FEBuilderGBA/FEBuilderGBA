
	.thumb

	gEventSlot = 0x030004B8

	gMapMovement = 0x0202E4E0
	gMapRange = 0x0202E4E4

@.equ MemorySlot3,0x30004C4    @item ID to give @[0x30004C4]!!?

	BmMapFill = 0x080197E4+1
	FillRangeMapForDangerZone = 0x0801B810+1

	.global CheckInDanger
	.type   CheckInDanger, function

CheckInDanger:
	push {r4-r7, lr}
	ldr r3, =FillRangeMapForDangerZone
	mov r0, #0 @ arg r0 = staff range?
	bl bxr3

	ldr r3, =BmMapFill 
	ldr r0, =gMapMovement @ arg r0 = gMapMovement
	ldr r0, [r0]
	mov r1, #1
	neg r1, r1            @ arg r1 = -1
	bl bxr3

	ldr r3, =gEventSlot
	ldr r1, [r3, #4*0x0B]

	lsr r2, r1, #16 @ r2 = slotB.y
	lsl r1, r1, #16
	lsr r1, r1, #16 @ r1 = slotB.x

	ldr r0, =gMapRange
	ldr r0, [r0]
	lsl r2, #2
	ldr r0, [r0, r2]
	ldrb r0, [r0, r1]

	str r0, [r3, #4*0x0C]

	pop {r4-r7}
	pop {r3}
bxr3:	bx r3
