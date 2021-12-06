.thumb
.include "../_TargetSelectionDefinitions.s"

.set pTCS_PositionSelectCursor, 0x085A0EA0	@{U}
@.set pTCS_PositionSelectCursor, 0x085C93C0	@{J}

FreeSelect6C_Init:
	push 	{r4, lr}
	
	mov 	r4, r0
	
	 _blh LockGameLogic
	
	ldr 	r0, =pTCS_PositionSelectCursor
	mov 	r1, #0
	
	_blh TCS_New
	str 	r0, [r4, #0x30]
	mov 	r1, #0x0
	
	@ vesly added 
	str 	r1, [r4, #0x68] @ empty rotation on init 
	
	strh 	r1, [r0, #0x22]
	
	ldr 	r3, =pGameDataStruct
	ldr 	r2, =ppRangeMapRows
	ldr 	r2, [r2]
	ldrh 	r0, [r3, #0x16]
	lsl 	r0, #0x2
	add 	r1, r0, r2
	ldr 	r1, [r1]
	ldrh 	r0, [r3, #0x14]
	add 	r1, r1, r0
	ldrb 	r0, [r1]
	mov 	r1, #0x0
	cmp 	r0, #0x0
	bne SetCursor
	
	@switch to x cursor if the cursor starts on an unselectable tile
	ldr 	r0, [r4, #0x30]
	mov 	r1, #0x1
	
	SetCursor:
	_blh TCS_SetAnim
	
	pop 	{r4}
	
	pop 	{r0}
	bx 	r0

.ltorg
.align

EALiterals:
	@ noting
