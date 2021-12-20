.thumb

@r4 = next unit to check in faction for valid cursor target
@r5 = cursor unit allegiance -> allegiance +1
@r6 = faction
@r7 = faction cap for memory


.global FindNextCursorUnit
.type FindNextCursorUnit, %function


		FindNextCursorUnit:
		push	{r4-r7,r14}
		mov		r5, r0
		
		@determine faction
		mov		r6, #0x80
		and		r0, r6
		cmp		r0, #0
		bne		CycleEnemyFaction		
		
		mov		r0, r5
		mov		r6, #0x40
		and		r0, r6
		cmp		r0, #0
		bne		CycleNPCFaction
		
		@Player Faction
		mov		r6, #0
		mov		r7, #0x3E
		b		CheckIfPastFaction
		
		CycleEnemyFaction:
		mov		r7, #0xB2
		b		CheckIfPastFaction
		
		CycleNPCFaction:
		mov		r7, #0x54
		
		CheckIfPastFaction:
		add		r5, #1
		mov		r4, r5
		cmp		r5, r7
		bgt		FirstUnitInFaction
		
		CheckIfValidForLPressCursorLoop:
		mov		r0, r4
		ldr		r3, CheckIfValidForLPressCursor
		mov		lr, r3
		.short	0xF800
		
		lsl		r0, #0x18
		cmp		r0, #0
		bne		End
		
		add		r4, #1
		cmp		r4, r7
		ble		CheckIfValidForLPressCursorLoop
		
		@Check if units at beginning of the faction still need to be checked (if loop started with a unit that was not the first unit of the faction)
		cmp		r5, r7
		beq		End
		
		mov		r7, r5
		
		FirstUnitInFaction:
		mov		r4, #1
		add		r4, r6
		cmp		r4, r5
		ble		CheckIfValidForLPressCursorLoop
		
		End:
		pop		{r4-r7}
		pop		{r0}
		bx		r0


		.align
		
		CheckIfValidForLPressCursor:
		.long	0x0801DADC


