.thumb

.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

push {r3,r4,r5,r6,r7} //NewWEXPRoutine
push {r2}
ldr r2, =lWEXPRFix
ldr r2, [r2, #0x0]
mov r4, r2
ldrb r2, [r4, #0x0]
cmp r2, #0x1
bne WEXPRInit

mov r1, #0x0
strb r1, [r4]

pop {r3}
ldr r3,=lWEXPRFix
ldr r3, [r3]
add r3, #0x1
ldrb r3, [r3]

ldr r1, [r7, #0x0] @Loads that unit's location
ldrb r1, [r1, #0x4] @Loads that unit's character ID byte
lsl r1, r1, #0x4
ldr r2,=StartWEXP
ldr r2, [r2, #0x0]
add r1, r1, r2  @Loads the current units new weapon xp xword
add r1, r1, r3 @Loads the current units current weapon
ldrb r2, [r1]
add r2, r2, r0
mov r0, #0x0
push {r1}
mov r1, r6
XPLoop:

ldr r3, =lMaxRank
ldrb r3, [r3, #0x0]
cmp r1, r3 @This hex is your maximum weapon rank
bge Skip

cmp r1, #0x0
bgt R1

Skip:
b RE

WEXPRInit:
pop {r2}
ldr r3,=lWEXPRFix
ldr r3, [r3]
add r3, #0x1
strb r2, [r3]
mov r1, #0x1
strb r1, [r4]
push {r14}
blh lEXPRCheck, r1
pop {r2}
pop {r3,r4,r5,r6,r7}
bx r2

REnd2:
b REnd
	R1: @//Start of first weapon rank
	cmp r1, #0x1
	bgt R2 
		ldr r3,=lWEXPRank1
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
		blt REnd2 @Check for if the xp has moved past the threshold
			add r0, #0x1 @Increase Weapon Rank
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
			b XPLoop @/Restart the xp check
	b REnd

	R2:
	cmp r1, #0x2
	bgt R3 
		ldr r3,=lWEXPRank2
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 2@ADDRESS
			b XPLoop 
	b REnd	

	R3:
	cmp r1, #0x3   
	bgt R4
		ldr r3,=lWEXPRank3
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 3@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 3@ADDRESS
			b XPLoop 
	b REnd

	R4:
	cmp r1, #0x4
	bgt R5
		ldr r3,=lWEXPRank4
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 4@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 4@ADDRESS
			b XPLoop 
	b REnd
	
	R5:
	cmp r1, #0x5
	bgt R6 
		ldr r3,=lWEXPRank5
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 5@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 5@ADDRESS
			b XPLoop 
	b REnd	
	
	R6:
	cmp r1, #0x6
	bgt R7 
		ldr r3,=lWEXPRank6
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 6@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 6@ADDRESS
			b XPLoop 
	b REnd	
	
	R7:
	cmp r1, #0x7
	bgt R8 
		ldr r3,=lWEXPRank7
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 7@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 7@ADDRESS
			b XPLoop 
	b REnd	

	R8:
	cmp r1, #0x8
	bgt R9 
		ldr r3,=lWEXPRank8
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 8@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 8@ADDRESS
			b XPLoop 
	b REnd	

	R9:
	cmp r1, #0x9
	bgt RA 
		ldr r3,=lWEXPRank9
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank 9@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank 9@ADDRESS
			b XPLoop 
	b REnd	

	RA:
	cmp r1, #0xA
	bgt RB 
		ldr r3,=lWEXPRankA
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank A@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank A@ADDRESS
			b XPLoop 
	b REnd	

	RB:
	cmp r1, #0xB
	bgt RC 
		ldr r3,=lWEXPRankB
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank B@ADDRESS
		blt REnd 
			add r0, #0x1
			add r1, #0x1 
			sub r2, r2, r3 @//Weapon Exp Required for Rank B@ADDRESS
			b XPLoop 
	b REnd	

	RC:
	cmp r1, #0xC
	bgt RD 
		ldr r3,=lWEXPRankC
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank C@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			sub r2, r2, r3 @//Weapon Exp Required for Rank C@ADDRESS
			b XPLoop 
	b REnd	

	RD:
	cmp r1, #0xD	
	bgt RE
		ldr r3,=lWEXPRankD
		ldrb r3, [r3, #0x0]
		cmp r2, r3 @//Weapon Exp Required for Rank D@ADDRESS
		blt REnd 
			add r0, #0x1 
			add r1, #0x1
			mov r2, #0x0
			b REnd
	b REnd	

	RE:
	mov r2, #0x0
	b REnd	

REnd:
pop {r1,r3,r4,r5,r6,r7}
strb r2,[r1]
bx r14
