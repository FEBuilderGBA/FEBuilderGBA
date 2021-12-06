.thumb
.include "../_TargetSelectionDefinitions.s"



FreeSelect6C_Loop:
	push 	{r4-r6, r14}

	mov 	r4, r0		@hold onto 6c pointer

	ldr 	r0, =pGameDataStruct
	ldr 	r5, [r0, #0x14]
@cursor movement
	_blh HandlePPCursorMovement

@ (r1, r2) = Cursor Map Pos
	ldr 	r0, =pGameDataStruct
	ldrh 	r1, [r0, #0x14]
	ldrh 	r2, [r0, #0x16]

@ r0 = New Key Presses
	ldr  r0, =pKeyStatusBuffer
	ldrh r0, [r0, #8] @ New Presses

@check for button presses
A_Press_Check:
	mov 	r3, #0x01 @ check if A button was pressed
	tst 	r0, r3
	beq 	NoAPress

	ldr 	r3, [r4, #0x2C]
	ldr 	r3, [r3, #0x0C] @ OnAPress

	cmp 	r3, #0x0
	beq NoAPress

	mov 	r0, r4
	bl BXR3

	b HandleCode

NoAPress:
	mov 	r3, #0x02 @ check if B button was pressed
	tst 	r0, r3
	beq 	NoBPress

	ldr 	r3, [r4, #0x2C]
	ldr 	r3, [r3, #0x10] @ OnBPress

	cmp 	r3, #0x0
	beq NoBPress

	mov 	r0, r4
	bl BXR3

	b HandleCode

NoBPress:
	mov 	r3, #0x1
	lsl 	r3, #0x8	@ check if R button was pressed
	tst 	r0, r3
	beq 	NoRPress
	
	
	
	b RotateMaskClockwise
	
	
	ldr 	r3, [r4, #0x2C]
	ldr 	r3, [r3, #0x14] @ OnRPress
	mov r6, r5 
	
	cmp 	r3, #0x0
	beq 	NoRPress

	mov 	r0, r4
	bl BXR3


	b HandleCode

NoRPress:
	mov 	r3, #0x1
	lsl 	r3, #0x9	@ check if L button was pressed
	tst 	r0, r3
	beq 	NoLPress
	

b RotateMaskCounterClockwise

NoLPress:


	ldr 	r0, =pGameDataStruct
	ldr 	r0, [r0, #0x14] @ r0 = Cursor Position Pair
	mov r6, r5 @ cursor coords 
	cmp 	r0, r5
	beq NoCursorMovement
	
	

	
	
	ldr 	r3, [r4, #0x2C] @ routine array pointer
	ldr 	r3, [r3, #0x08] @ OnPositionChange
	
	cmp 	r3, #0
	beq NoCursorMovement
	
	mov 	r0, r4
	bl BXR3

HandleCode:
	mov 	r5, r0
	
	mov 	r0, #2
	tst 	r5, r0
	beq NoDelete
	
	@ Breaking loop
	mov 	r0, r4
	_blh Break6CLoop
	
	ldr 	r3, [r0, #0x2C]@ routine array pointer
	ldr 	r3, [r3, #0x04]
	
	cmp 	r3, #0
	beq 	NoCall
	
	mov 	r0, r4
	bl BXR3
	
NoCall:
	ldr 	r0, [r4, #0x30]
	_blh TCS_Free
	
	b End @ No need to draw, so go directly to end
	
NoDelete:
	ldr  	r0, =pChapterDataStruct
	add  	r0, #0x41
	ldrb 	r0, [r0]
	
	@ Options set to "no sound effect"
	lsl 	r0, #0x1E
	cmp 	r0, #0x0
	blt 	NoSound
	
	mov 	r0, #4
	tst 	r5, r0
	beq 	NoBeep
	
	mov 	r0, #0x6A
	_blh PlaySoundEffect
	
NoBeep:
	mov 	r0, #8
	tst 	r5, r0
	beq NoBoop
	
	mov 	r0, #0x6B
	_blh PlaySoundEffect
	
NoBoop:
	mov 	r0, #0x10 
	tst 	r5, r0
	beq NoGurr
	
	mov 	r0, #0x6C
	_blh PlaySoundEffect
	
NoGurr:
NoSound:
	mov 	r0, #0x20
	tst 	r5, r0
	beq XCursor

@ y cursor movement 
	ldr r0, [r4, #0x30]
	mov r1, #0x0
	
	_blh TCS_SetAnim
	
	
	b Finish
	XCursor:
	mov 	r0, #0x40
	tst 	r5, r0
	beq NoCursorMovement
@ x cursor movement 
	ldr r0, [r4, #0x30]
	mov r1, #0x1
	
	_blh TCS_SetAnim
	
	

	
	
NoCursorMovement:
Finish:
	@ Update Cursor Graphics
	
	ldr r3, =pGameDataStruct
	
	@ Cursor Gfx X
	mov r0, #0x20
	ldsh r1, [r3, r0]
	
	@ Camera X
	mov r0, #0x0C
	ldsh r2, [r3, r0]
	
	@ Draw X
	sub r1, r2
	
	@ Cursor Gfx Y
	mov r0, #0x22
	ldsh r2, [r3, r0]
	
	@ Camera Y
	mov r0, #0x0E
	ldsh r3, [r3, r0]
	
	@ Draw Y
	sub r2, r3
	
	ldr r0, [r4, #0x30]
	_blh TCS_Update
	
@ vesly 
lsl r0, r6, #16 
lsr r0, #16 
lsl r1, r6, #8 
lsr r1, #24 
mov r3, r4 
add r3, #0x68 @ rotation byte - 2b or 0x68 ? 
ldrb 	r2, [r3]
@lsl r2, #30 
@lsr r2, #30 
bl AoE_CallDisplayDamageArea
b End 


RotateMaskCounterClockwise:
@b DisplayMask
mov r3, r4 
add r3, #0x68 @ rotation byte 
ldrb 	r2, [r3]
lsl r2, #30 
lsr r2, #30 
cmp r2, #0
bgt SubOne
mov r2, #3 
strb r2, [r3] 
b DisplayMask
SubOne:
sub r2, #1 
strb r2, [r3] 
b DisplayMask


RotateMaskClockwise:
@b DisplayMask
mov r3, r4 
add r3, #0x68 @ rotation byte 
ldrb 	r2, [r3]
lsl r2, #30 
lsr r2, #30 
cmp r2, #3 
blt AddOne
mov r2, #0 
strb r2, [r3] 
b DisplayMask
AddOne:
add r2, #1 
strb r2, [r3] 
b DisplayMask

DisplayMask:
lsl r0, r6, #16 
lsr r0, #16 
lsl r1, r6, #8 
lsr r1, #24 
@r2 is still rotation byte 
bl AoE_CallDisplayDamageArea
b HandleCode

End:
pop 	{r4-r6}
pop 	{r3}

BXR3:
bx	r3

.ltorg
.align

OffsetList:
