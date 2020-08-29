.thumb

	

ldr r1, [r7, #0x0] @Loads that unit's location
ldrb r1, [r1, #0x4] @Loads that unit's character ID byte
lsl r1, r1, #0x4
ldr r2,=StartWEXP
ldr r2, [r2, #0x0]
add r1, r1, r2  @Loads the current units new weapon xp xword
ldrb r2, [r1, #0x0]
add r2, r2, r0
mov r0, #0x0
push {r1}
mov r1, r6
XPLoopC:

ldr r3, =lMaxRank
ldrb r3, [r3, #0x0]
cmp r1, r3 @This hex is your maximum weapon rank
bge SkipC

cmp r1, #0x0
bgt R1C

SkipC:
b REndC

	R1C: @//Start of first weapon rank
	cmp r1, #0x1
	bgt R2C 
		ldr r3,=lWEXPRank1
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
		blt SkipC @Check for if the xp has moved past the threshold
			add r0, #0x1 @Increase Weapon Rank
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
			b XPLoopC @/Restart the xp check
	b REndC

	R2C:
	cmp r1, #0x2
	bgt R3C 
		ldr r3,=lWEXPRank2
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
			b XPLoopC
	b REndC	

	R3C:
	cmp r1, #0x3   
	bgt R4C
		ldr r3,=lWEXPRank3
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 3@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 3@ADDRESS
			b XPLoopC 
	b REndC

	R4C:
	cmp r1, #0x4
	bgt R5C
		ldr r3,=lWEXPRank4
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 4@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 4@ADDRESS
			b XPLoopC 
	b REndC
	
	R5C:
	cmp r1, #0x5
	bgt R6C 
		ldr r3,=lWEXPRank5
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 5@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 5@ADDRESS
			b XPLoopC 
	b REndC	
	
	R6C:
	cmp r1, #0x6
	bgt R7C 
		ldr r3,=lWEXPRank6
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 6@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 6@ADDRESS
			b XPLoopC 
	b REndC	
	
	R7C:
	cmp r1, #0x7
	bgt R8C 
		ldr r3,=lWEXPRank7
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 7@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 7@ADDRESS
			b XPLoopC 
	b REndC	

	R8C:
	cmp r1, #0x8
	bgt R9C 
		ldr r3,=lWEXPRank8
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 8@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 8@ADDRESS
			b XPLoopC 
	b REndC	

	R9C:
	cmp r1, #0x9
	bgt RAC 
		ldr r3,=lWEXPRank9
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 9@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 9@ADDRESS
			b XPLoopC 
	b REndC	

	RAC:
	cmp r1, #0xA
	bgt RBC 
		ldr r3,=lWEXPRankA
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank A@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank A@ADDRESS
			b XPLoopC 
	b REndC	

	RBC:
	cmp r1, #0xB
	bgt RCC 
		ldr r3,=lWEXPRankB
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank B@ADDRESS
		blt REndC 
			add r0, #0x1
			add r1, #0x1 
			sub r2, r2, r3 @//Weapon Exp Required for Rank B@ADDRESS
			b XPLoopC 
	b REndC	

	RCC:
	cmp r1, #0xC
	bgt RDC 
		ldr r3,=lWEXPRankC
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank C@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank C@ADDRESS
			b XPLoopC 
	b REndC	

	RDC:
	cmp r1, #0xD	
	bgt REC
		ldr r3,=lWEXPRankD
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank D@ADDRESS
		blt REndC 
			add r0, #0x1 
			add r1, #0x1
			mov r2, #0x0
			b REndC
	b REndC	

	REC:
	mov r2, #0x0
	b REndC

REndC:
pop {r1}
bx lr