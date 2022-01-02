.thumb

@r0
@r1
@r2
@r3
@r4  sort pointer
@r5   sort pointer2
@r6   sort order Table
@r7
@r8
@r9  
@r10 term pointer
@Hook 0809A40C	{J}
@Hook 08098134	{U}

	ldr r6, ItemSortOrderTable
	
	ldr r0,  =0x02012914	@sort Buffer {J}	{U}
							@strcut {  byte unk , byte index, item id, item count} //sieof==4
	mov r4, r0

	ldr r1,  =0x02012F56	@ListCount {J}	{U}
	ldrb r1, [r1]
	lsl r1, #0x2    @sizeof()==4
	add r1, r4
	mov r10, r1     @ソート終端
	
	SortLoop:
	add r5, r4, #0x4    @next value. sizeof()==4
	cmp r5, r10
	bge Exit

	SortOne:
	ldrb r1, [r4,#0x2]  @ItemID
	ldrb r0, [r5,#0x2]  @ItemID

	CheckSortOrder:
	ldrb r2, [r6,r1]
	ldrb r3, [r6,r0]
	cmp r2 ,r3
	bgt Swap
	bne Next

	CheckItemID:
	cmp r1 ,r0
	bgt Swap
	bne Next

	CheckItemUses:
	ldrb r0, [r4, #0x3] @耐久
	ldrb r1, [r5, #0x3] @耐久
	cmp r0 ,r1
	bls Next
    
	Swap:
	ldr r1, [r4, #0x0]
	ldr r0, [r5, #0x0]
	str r0, [r4, #0x0]
	str r1, [r5, #0x0]

	Next:
	add r5, #0x4    @next value. sizeof()==4
	cmp r5, r10
	blt SortOne

	add r4, #0x4    @next value. sizeof()==4
	b   SortLoop

	Exit:
	ldr r5,  =0x02012914	@sort Buffer {J}	{U}
@	ldr r3, =0x0809A494|1	@{J}
	ldr r3, =0x080981BC|1	@{U}
	bx r3

.align
.ltorg
ItemSortOrderTable:
