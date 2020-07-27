.thumb

.global MS_SaveExpandedWMData
.type MS_SaveExpandedWMData, %function
.global MS_SaveWMNodes
.type MS_SaveWMNodes, %function
.global MS_LoadExpandedWMData
.type MS_LoadExpandedWMData, %function

.global SaveWM_ShiftByNodeDatasize
.type SaveWM_ShiftByNodeDatasize, %object
.global SaveWM_NodeCount
.type SaveWM_NodeCount, %object
.global LoadWMPaths_PathsStartOffset1
.type LoadWMPaths_PathsStartOffset1, %object
.global LoadWMPaths_PathsStartOffset2
.type LoadWMPaths_PathsStartOffset2, %object
.global LoadWMPaths_PathsSize
.type LoadWMPaths_PathsSize, %object

.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.align

MS_SaveExpandedWMData:
	push {r4-r7,lr}   //SaveWMStuff
	sub 	sp, #0x30
	mov 	r6, r0 				@sram offset
	mov 	r4, r1 				@source data 03005280
	mov 	r0, r4
	mov 	r1, sp
	bl 		MS_SaveWMNodes 		@replaces 0x080A6DA0
	add 	r1, sp, #0x10 		@start of path data
	mov 	r0, r4
	blh 	0x080A6EB0			@vanilla SaveWMPaths
	add 	r1, sp, #0x18 		@start of actor data
	mov 	r0, r4
	blh 	0x080A6F50			@vanilla SaveWMActorData
	mov 	r1, sp
	add 	r1, #0x29 			@skirmish array sram start offset
	mov 	r0, r4
	blh 	0x080A7034			@vanilla SaveWMSkirmishData
	mov 	r0, r4
	add 	r0, #0xcc
	ldrb 	r1, [r0, #0x0]
	add 	r0, sp, #0x2C		@byte 0xCC is saved to this offset
	strb 	r1, [r0, #0x0]
	mov 	r1, sp
	mov 	r7, #0x26			@cursor data offset
	ldrh 	r0, [r4, #0x2]
	strb 	r0, [r1, r7] 		@cursor x
	add 	r7, #0x1 			@0x21
	ldrh 	r0, [r4, #0x4]
	strb 	r0, [r1, r7] 		@cursor y
	add 	r7, #0x1			@0x22 (flags offset)
	mov 	r5, sp
	ldrb 	r3, [r4, #0x0]
	
	@Flag stuff is here if we need to change which get saved.
	
	lsl 	r0, r3, #0x1e
	lsr 	r0, r0, #0x1f
	ldrb 	r1, [r5, r7] 
	mov 	r2, #0x2
	neg 	r2, r2
	and 	r2, r1
	orr 	r2, r0
	strb 	r2, [r5, r7] 
	mov 	r4, sp
	lsl 	r1, r3, #0x1d
	lsr 	r1, r1, #0x1f
	lsl 	r1, r1, #0x1
	mov 	r0, #0x3
	neg 	r0, r0
	and 	r0, r2
	orr 	r0, r1
	strb 	r0, [r4, r7] 
	mov 	r2, sp
	lsl 	r3, r3, #0x1a
	lsr 	r3, r3, #0x1e
	lsl 	r3, r3, #0x2
	mov 	r1, #0xd
	neg 	r1, r1
	and 	r0, r1
	orr 	r0, r3
	strb 	r0, [r2, r7] 
	mov 	r0, sp
	mov 	r1, r6
	mov 	r2, #0x30
	blh 	0x080D184C   	@SRAMTransfer_WithCheck
	add 	sp, #0x30
	pop 	{r4-r7}
	pop 	{r0}
	bx 		r0

.align
.ltorg

MS_SaveWMNodes:
	push 	{r4-r7,lr}
	mov 	r7, r9
	mov 	r6, r8
	push 	{r6,r7}
	mov 	r9, r0
	mov 	r7, r1
	mov 	r1, #0x0
	mov 	r0, r7
	add 	r0, #0xf		@data size
	ClearLoop1:
		strb 	r1, [r0, #0x0]
		sub 	r0, #0x1
		cmp 	r0, r7
		bge 	ClearLoop1
	mov 	r5, #0x0
	mov 	r0, #0x0
	mov 	r1, #0x1
	mov 	r8, r1
	NodeLoopStart2:
	mov 	r4, #0x0
	add 	r1, r0, #0x1
	mov 	r12, r1
	SaveWM_ShiftByNodeDatasize:
	lsl 	r0, r0, #0x2 	@change as needed
	add 	r0, r9
	mov 	r6, r0
	add 	r6, #0x30
	NodeLoopStart1:
	cmp 	r4, #0x0
	beq FirstPass1
		cmp 	r4, #0x1
		beq 	SecondPass1
			b 		SubsequentPasses1
		FirstPass1:
		ldrb 	r0, [r6, #0x0]
		mov 	r3, r8
		and 	r3 ,r0
		b 		SubsequentPasses1
	SecondPass1:
	ldrb 	r1, [r6, #0x0]
	mov 	r0, #0x2
	and 	r0, r1
	lsl 	r0, r0, #0x18
	lsr 	r3, r0, #0x18
	SubsequentPasses1:
	lsl 	r0, r3, #0x18
	cmp 	r0, #0x0
	beq MoveNext1
		mov 	r2, r5
		cmp 	r5, #0x0
		bge NotNegative1
			mov 	r2, r5
			add 	r2, #0x7 @does this need to be 0xF?
		NotNegative1:
		asr 	r2, r2, #0x3
		mov 	r1, #0x7	@this is used for bitpacking
		and 	r1, r5
		add 	r2, r7, r2
		mov 	r0, r8
		lsl 	r0, r1
		ldrb 	r1, [r2, #0x0]
		orr 	r0, r1
		strb 	r0, [r2, #0x0]
	MoveNext1:
	add 	r5, #0x1
	add 	r4, #0x1
	cmp 	r4, #0x1
	ble 	NodeLoopStart1
	mov 	r0, r12
	SaveWM_NodeCount:
	cmp 	r0, #0x35
	ble 	NodeLoopStart2
	pop 	{r3,r4}
	mov 	r8, r3
	mov 	r9, r4
	pop 	{r4-r7}
	pop 	{r0}
	bx 		r0

.align
.ltorg

BXR3:
bx		r3

.align

MS_LoadExpandedWMData:
	push {r4,r5,lr}
	sub 	sp, #0x30
	mov 	r4, r1			@WMDataStruct
	ldr 	r1, =0x030067A0 @gpReadSramFast
	ldr 	r3, [r1, #0x0]
	mov 	r1, sp
	mov 	r2, #0x2c
	bl 		BXR3   
	mov 	r0, r4
	mov 	r1, sp
	blh		0x080A6E24 		@vanilla LoadWMNodes	
	add 	r1, sp, #0x10
	mov 	r0, r4
	bl		MS_LoadWMPaths	@was A6F0C
	add 	r1, sp, #0x18
	mov 	r0, r4
	blh 	0x080A6FBC 		@vanilla LoadWMActors
	mov 	r1, sp
	add 	r1, #0x29
	mov 	r0 ,r4
	blh 	0x080A7054 		@vanilla LoadWMSkirmishData
	add 	r0, sp, #0x2C
	ldrb 	r1, [r0, #0x0]
	mov 	r0 ,r4
	add 	r0, #0xcc
	strb 	r1, [r0, #0x0]
	mov 	r0, sp
	mov 	r5, #0x28		@flags offset
	ldrb 	r1, [r0, r5]
	mov 	r0, #0x1
	and 	r0 ,r1
	cmp 	r0, #0x0
	beq 	NotLoadingOnWorldmap
		ldrb 	r0, [r4, #0x0]
		mov 	r1, #0x2
		orr 	r0, r1
		b 		FlagLoad1
	NotLoadingOnWorldmap:
	ldrb 	r1, [r4, #0x0]
	mov 	r0, #0x3
	neg 	r0, r0
	and 	r0, r1
	FlagLoad1:
	strb 	r0, [r4, #0x0]
	mov 	r0, sp
	ldrb 	r1, [r0, r5]
	mov 	r0, #0x2
	and 	r0, r1
	cmp 	r0, #0x0
	beq 	NotLoadingFreeroam
		ldrb 	r0, [r4, #0x0]
		mov 	r1, #0x4
		orr 	r0 ,r1
		b 		FlagLoad2
	NotLoadingFreeroam:
	ldrb 	r1, [r4, #0x0]
	mov 	r0, #0x5
	neg 	r0, r0
	and 	r0, r1
	FlagLoad2:
	strb 	r0, [r4, #0x0]
	mov 	r0, sp
	ldrb 	r1, [r0, r5]
	
	@Flag stuff is here if we need to change which get loaded.
	
	lsl 	r1, r1, #0x1c
	lsr 	r1, r1, #0x1e
	lsl 	r1, r1, #0x4
	ldrb 	r2, [r4, #0x0]
	mov 	r0, #0x31
	neg 	r0, r0
	and 	r0, r2
	orr 	r0, r1
	strb 	r0, [r4, #0x0]
	
	@end of flags section
	
	mov 	r0, sp
	mov 	r5, #0x26		@cursor x
	ldrb 	r0, [r0, r5]
	strh 	r0, [r4, #0x2]
	mov 	r0, sp
	add 	r5, #0x1
	ldrb 	r0, [r0, r5]
	strh 	r0, [r4, #0x4]
	add 	sp, #0x30
	pop 	{r4,r5}
	pop 	{r0}
	bx 		r0

.align
.ltorg

MS_LoadWMPaths:
	push 	{r4-r6, lr}
	mov		r5, r0
	mov 	r6, r1
	mov 	r1, r5
	mov 	r2, r5
	add 	r1, #0xC4		@paths count
	LoadWMPaths_PathsStartOffset1:
	add		r2, #0xA4		@paths start offset
	mov 	r0, #0x0
	strb 	r0, [r1]		@clear pathcount
	mov 	r0, #0xFF
	
	@need to clear out existing path data in WMDataStruct, as having it present with a nodecount of 0 causes SetupNewWMRoute_Fix to skip drawing the first route. This is only an issue when loading a suspend without powering off the console
	
	PathClearLoop:
		strb 	r0, [r2]
		add 	r2, #0x1
		cmp 	r2, r1
		blt		PathClearLoop
	mov r4, #0x0
	PathLoadLoop:
		mov r0, r4
		cmp r4, #0x0
		bge DontAdd
			add r0, r4, #0x7
		DontAdd:
		asr r0, r0, #0x3
		add r0, r6, r0
		ldrb r1, [r0]
		mov r0, #0x7
		and r0, r4
		asr r1, r0
		mov r0, #0x1
		and r1, r0
		cmp r1, #0x0
		beq MoveNextPath
			mov r0, r5
			mov r1, r5
			LoadWMPaths_PathsStartOffset2:
			add r1, #0xA4
			mov r2, r4
			blh 0x080BC8BC @SetupNewWMRoute
	MoveNextPath:
		add r4, #0x1
		LoadWMPaths_PathsSize:
		cmp r4, #0x1f
		ble PathLoadLoop
	pop {r4-r6}
	pop {r0}
	bx r0
