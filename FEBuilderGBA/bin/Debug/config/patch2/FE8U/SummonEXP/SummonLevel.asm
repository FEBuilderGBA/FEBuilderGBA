
.thumb

.equ LevelUpCalc, 0x802BA28
.equ Divide, 0x080D18FC

.global SummonLevel
.type SummonLevel, %function
SummonLevel: @ Autohook to 0x0802B980
@ From vanilla routine
strb r1, [ r4, #0x9 ] @ Last setting of something before checking for level up
mov r0, r5
blh LevelUpCalc, r2 @ Checks for attacker level up
mov r0, r4
blh LevelUpCalc, r2 @ Checks for defender level up

mov r0, r5
ldr r1, [ r5, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r2, =PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
beq WeGotAPhantom @ Attacker is a phantom
mov r0, r4
ldr r1, [ r4, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r2, =PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
bne EndSummonLevel @ No phantom found
@ Defender is a phantom

WeGotAPhantom:
push { r0 } @ Save the battle struct
ldrb r0, [ r0, #0x0B ]
blh GetUnit, r1 @ r0 = Phantom's character struct.
mov r4, r0 @ Save this for later
mov r1, #0xFF
strb r1, [ r0, #0x09 ] @ Set EXP in the phantom's character struct back to 0xFF.

bl FindSummoner @ r0 = Summoner's character struct.
mov r1, r0

pop { r0 } @ r0 has the phantom's battle struct.
mov r2, #0x6E
add r2, r0, r2
ldrb r2, [ r2 ] @ EXP change in r2.
ldrb r3, [ r1, #0x9 ] @ Summoner's EXP in r3
add r2, r3
strb r2, [ r1, #0x9 ] @ Summoner now has total new EXP
mov r3, #0xFF
strb r3, [ r0, #0x09 ] @ Set the phantom's EXP back to 0.

mov r3, #100
cmp r2, r3
blt EndSummonLevel
@ Damn it we need a level up.

@ So now I need to prepare for the level up check and the level up itself. I need to copy the phantom's struct into the whatever (attack or defense) struct,
	@ but I also need to put the phantom's character struct back for safe keeping.

push { r1 } @ Summoner's character struct
push { r0 } @ Phantom's battle struct
@ Character struct of the phantom was saved in r4.
Swap r1, r4 @ Character struct of the summoner is saved in r4 while the character struct of the phantom is in r1
@bl CopyStruct

@ Now I have to put the summoner character struct in the battle struct so the phantom doesn't get the level up.
@ r0 has the battle struct

Swap r1, r4 @ Now the phantom character struct is in r4, and the summoner character struct is in r1
Swap r0, r1 @ So now r0 = summoner's character struct, and r1 = battle struct
 bl CopyStruct

@ Whew. Now that the summoner's character struct is in the correct whatever struct, time to actually call the level up and hope it doesn't break.
 ldr r0, [ sp ] @ Puts the battle struct back into r0 without affecting the stack
 blh LevelUpCalc, r2

@ Now to put the battle struct with the summoner back into the character struct of the summoner
pop { r0 } @ r0 has the battle struct
pop { r1 } @ r1 has the character struct of the summoner
bl CopyStruct

@ Jesus. Now to put the character struct of the phantom BACK into the battle struct.
mov r1, r0 @ r1 has battle struct
mov r0, r4 @ r0 has character struct of the phantom
bl CopyStruct

EndSummonLevel:
pop { r4 - r6 }
pop { r0 }
bx r0
