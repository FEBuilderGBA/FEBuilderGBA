.thumb

.equ origin, 0x0803e320
.equ GetUnitStruct, . + 0x08019430 - origin
.equ AreUnitsAllied, . + 0x08024D8C - origin
.equ CanUnitUseAsWeapon, . + 0x08016574 - origin
.equ GetItemMight, . + 0x080175DC - origin
.equ CouldUnitBeInRangeHeuristic, . + 0x0803AC90 - origin
.equ FillMovementAndRangeMapForItem, . + 0x0803B558 - origin
.equ GetUnitPower, . + 0x080191B0 - origin

FillAIDangerMap:
	push 	{r4-r7,lr}
	mov 	r7, r10
	mov 	r6, r9
	mov 	r5, r8
	mov		r4, r11
	push 	{r4-r7}
	mov 	r0, #0x0
	mov 	r8, r0
	mov 	r9, r0
	mov 	r4, #0x1
label1:
	mov 	r0, r4
	bl 		GetUnitStruct
	mov 	r6, r0
	add 	r4, #0x1
	mov 	r10, r4
	cmp 	r6, #0x0
	beq 	label2 					@3e41c
		ldr 	r0, [r6, #0x0] 
		cmp 	r0, #0x0
		beq 	label2 				@3e41c
			ldr 	r0, [r6, #0xc] 
			ldr 	r1, =0x0001000d
			and 	r0, r1
			cmp 	r0, #0x0
			bne 	label2 			@3e41c
				ldr 	r0, =0x0202BE44 	@gActiveUnitIndex 
				ldrb 	r0, [r0, #0x0] 
				mov 	r1, #0xb
				ldsb 	r1, [r6, r1] 
				bl 		AreUnitsAllied
				cmp 	r0, #0x0
				bne 	label2 		@3e41c
					mov 	r5, #0x0
					ldrh 	r4, [r6, #0x1e] 
					cmp 	r4, #0x0
					beq 	label3 	@3E3A6
			label5:
				mov 	r0, r6
				mov 	r1, r4
				bl 		CanUnitUseAsWeapon
				cmp 	r0, #0x0
				beq 	label4 		@3e392
					mov 	r0, r4
					bl 		GetItemMight
					cmp 	r0, r9
					ble 	label4 		@3e392
						mov 	r8, r4
						mov 	r0, r8
						bl 		GetItemMight
						mov 	r9, r0
				label4:
					add 	r5, #0x1
					cmp 	r5, #0x4
					bgt 	label3 		@3e3a6
					lsl 	r1, r5, #0x1
					mov 	r0, r6
					add 	r0, #0x1e
					add 	r0, r0, r1
					ldrh 	r4, [r0, #0x0] 
					cmp 	r4, #0x0
					bne 	label5 		@3e36c
				
		label3:
			mov 	r1, r8
			cmp 	r1, #0x0
			beq 	label2 		@3e41c
			
				@New addition, caching the unit's power
				mov 	r0, r6
				bl 		GetUnitPower
				mov 	r11, r0
			
				ldr 	r0, =0x03004e50 
				ldr 	r0, [r0, #0x0] 
				mov 	r1, r6
				mov 	r2, r8
				bl 		CouldUnitBeInRangeHeuristic
				cmp 	r0, #0x0
				beq 	label2 		@3e41c
					mov 	r0, r6
					mov 	r1, r8
					bl 		FillMovementAndRangeMapForItem
					ldr 	r0, =0x0202E4D4 	@MapSize
					mov 	r2, #0x2
					ldsh 	r0, [r0, r2] 		@MapSize.Height
					sub 	r1, r0, #0x1
					cmp 	r1, #0x0
					blt 	label2 		@3e41c
				
				
			label9:
				ldr 	r0, =0x0202E4D4 		@MapSize
				mov 	r2, #0x0
				ldsh 	r0, [r0, r2] 			@MapSize.width
				sub 	r4, r0, #0x1
				sub 	r7, r1, #0x1
				cmp 	r4, #0x0
				blt 	label6 					@3e416
					lsl r5, r1, #0x2
							
		label8:
			ldr 	r0, =0x0202E4E4 		@gMapRange
			ldr 	r0, [r0, #0x0]
			add 	r0, r5, r0
			ldr 	r0, [r0, #0x0]
			add 	r0, r0, r4
			ldrb 	r0, [r0, #0x0] 
			lsl 	r0, r0, #0x18
			asr 	r0, r0, #0x18
			cmp 	r0, #0x0
			beq 	label7 		@3e410
				mov 	r0, r11
				ldr 	r1, =0x0202E4F0 	@gMapMovement2
				ldr 	r1, [r1, #0x0] 
				add 	r1, r5, r1
				ldr 	r1, [r1, #0x0] 
				add 	r1, r1, r4
				add 	r0, r9
				asr 	r0, r0, #0x1
				ldrb 	r2, [r1, #0x0] 
				add 	r0, r0, r2
				strb 	r0, [r1, #0x0]
		label7:
			sub 	r4, #0x1
			cmp 	r4, #0x0
			bge 	label8 		@3e3e2
	
	label6:
		mov 	r1, r7
		cmp 	r1, #0x0
		bge 	label9 		@3e3d2
		
label2:
	mov 	r4, r10
	cmp 	r4, #0xBF
	ble 	label1 @3E332
	pop 	{r3-r6}
	/*
	mov 	r8, r3
	mov 	r9, r4
	mov 	r10, r5
	*/
	mov 	r11, r3
	mov 	r8, r4
	mov 	r9, r5
	mov 	r10, r6
	pop 	{r4-r7}
	pop 	{r0}
	bx 		r0

.align
.ltorg

