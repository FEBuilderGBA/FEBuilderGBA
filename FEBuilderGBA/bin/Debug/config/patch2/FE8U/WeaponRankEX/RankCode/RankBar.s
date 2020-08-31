.thumb

.macro SET_FUNC name, value
    .global \name
    .type   \name, %function
    .set    \name, \value
.endm

SET_FUNC StatScreenStruct, (0x02003C08)


push {r4,r5,r6,lr}   //GetWRankBarData
mov r3, r9
push {r3}
add sp, #0x40
ldrb r3, [sp]
sub sp, #0x40
cmp r3, #0x28
blt RBCont
cmp r3, #0x2f
bgt RBCont
sub r3, #0x28
mov r9, r3


RBCont:
mov r5 ,r0
mov r4 ,r1
mov r6 ,r2
bl WeaponLevel 	@0x08016d5c   //GetWeaponRankLevel
cmp r0, #0xE
bhi RBEnd4 	
    lsl r0 ,r0 ,#0x2
    ldr r1,=StartingBuffer
    add r0 ,r0, r1
    ldr r0, [r0, #0x0]
    	ldr r1,=StatScreenStruct @Loads current stat screen unit
   	ldr r1, [r1, #0x0] @Loads that unit's location
   	ldr r1, [r1, #0x0] @Loads that unit's character data
   	ldrb r1, [r1, #0x4] @Loads that unit's character ID byte
	cmp r1, #0x3E @Makes sure the unit is not an enemy unit
	bhi EnemyUnit
   		lsl r1, r1, #0x4
   		ldr r2,=StartWEXP
		ldr r2, [r2, #0x0]
   		add r1, r1, r2  @Loads the current units new weapon xp xword
   		add r1, r1, r9 @Adds the current weapon to check for weapon xp
   		ldrb r5, [r1, #0x0] @Loads the weapon xp into r5
		bl StartBarRoutine
	EnemyUnit:
	mov r5, #0x0
    StartBarRoutine:
    mov pc, r0
RBEnd4:
b RBEnd3

.align
StartingBuffer:
.WORD RBEnd1	@		08016E70 6EBC 0801   //LDRDATA
.WORD BRank1	@		08016E74 6E8C 0801   //SWITCH CASE
.WORD BRank2	@		08016E78 6E94 0801   //SWITCH CASE
.WORD BRank3	@		08016E7C 6E9E 0801   //SWITCH CASE
.WORD BRank4	@		08016E80 6EA8 0801   //SWITCH CASE
.WORD BRank5	@		08016E84 6EB2 0801   //SWITCH CASE
.WORD BRank6	@		08016E70 6EBC 0801   //SWITCH CASE
.WORD BRank7	@		08016E74 6E8C 0801   //SWITCH CASE
.WORD BRank8	@		08016E78 6E94 0801   //SWITCH CASE
.WORD BRank9	@		08016E7C 6E9E 0801   //SWITCH CASE
.WORD BRankA	@		08016E80 6EA8 0801   //SWITCH CASE
.WORD BRankB	@		08016E84 6EB2 0801   //SWITCH CASE
.WORD BRankC	@		08016E7C 6E9E 0801   //SWITCH CASE
.WORD BRankD	@		08016E80 6EA8 0801   //SWITCH CASE
.WORD RBEnd1	@		08016E88 6EBC 0801   //SWITCH CASE

@_______________________________________________________________________________________________
BRank1:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank1
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 1
    b RBEnd2	
BRank2:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank2
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 2
    b RBEnd2	
BRank3:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank3
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 3
    b RBEnd2	
BRank4:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank4
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 4
    b RBEnd2	
BRank5:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank5
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 5
    b RBEnd2	
BRank6:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank6
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 6
    b RBEnd2	
BRank7:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank7
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 7
    b RBEnd2	
BRank8:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank8
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 8
    b RBEnd2	
BRank9:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRank9
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank 9
    b RBEnd2	
BRankA:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRankA
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank A 
    b RBEnd2	
BRankB:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRankB
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank B
    b RBEnd2	
BRankC:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRankC
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank C
    b RBEnd2	
BRankD:
    mov r0 ,r5
    str r0, [r4, #0x0]
    ldr r1,=lWEXPRankD
    ldrb r1, [r1, #0x0]
    mov r0, r1		//Weapon Exp Required for Rank D
    b RBEnd2	

@______________________________________________________________________________________________

RBEnd1:
    mov r0, #0x0
    str r0, [r4, #0x0]
RBEnd2:
    str r0, [r6, #0x0]
RBEnd3:
pop {r3}
mov r9, r3
pop {r4,r5,r6}
pop {r0}
bx r0

