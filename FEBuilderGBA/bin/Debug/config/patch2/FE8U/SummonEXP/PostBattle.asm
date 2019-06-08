
@ It seems that when the growths from the level up are added, the battle struct is untouched?
@ It writes directly into the character struct starting at 0x0802C22C. This is very simple. 
	@ It just grabs the previous stat and the growth, adds them, then sets the new stat into the character struct.
@ The proc seems to be handled elsewhere....
	@ I bet that the 0x0807EDA4 area has something to do with it.
	
@ Hook to 0x0802C220
@ Return to 0x0802C27A

@ r4 = character struct (of phantom)
@ r5 = battle struct

.equ CheckCaps, 0x80181C8

.global FixSettingGrowths
.type FixSettingGrowths, %function
FixSettingGrowths:
push { r4, r5 }
mov r0, r5 @ r0 now has battle struct
ldr r0, [ r5, #0x04 ]
ldrb r1, [ r0, #0x04 ]
ldr r0, =PhantomIDSummonASM
ldrb r0, [ r0 ]
cmp r1, r0
bne NormalGrowths
@ So it's a phantom. This means I have to put the summoner's character struct in.
mov r0, r4
bl FindSummoner @ r0 = Summoner's character struct.

mov r3, r4 @ Phantom's battle struct.
mov r4, r0 @ Summoner's character struct.

NormalGrowths:
mov r0, r5
add r0, #0x73
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x12 ]
add r0, r1
strb r0,[ r4, #0x12 ]
mov r0, r5
add r0, #0x74
ldrb r0,[ r0 ]
ldrb r1, [ r4, #0x14 ]
add r0, r1
strb r0, [ r4, #0x14 ]
mov r0, r5
add r0, #0x75
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x15 ]
add r0, r1
strb r0, [ r4, #0x15 ]
mov r0, r5
add r0, #0x76
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x16 ]
add r0, r1
strb r0, [ r4, #0x16 ]
mov r0, r5
add r0, #0x77
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x17 ]
add r0, r1
strb r0, [ r4, #0x17 ]
mov r0, r5
add r0, #0x78
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x18 ]
add r0, r1
strb r0, [ r4, #0x18 ]
mov r0, r5
add r0, #0x79
ldrb r0, [ r0 ]
ldrb r1, [ r4, #0x19 ]
add r0, r1
strb r0, [ r4, #0x19 ]
mov r0, r4
blh CheckCaps, r2

pop { r4, r5 }
ldr r1, =#0x0802C27B
bx r1

@ Hook at 0x0807ED38
@ Return to 0x0807ED4E
@ I don't think I actually need to set the phantom's character struct back. It's automatically overwritten.
@ Also fuck this function.

.global SetUpSummonProc
.type SetUpSummonProc, %function
SetUpSummonProc: @ r0 = ?, r1 = which stat is being calculated (HP, str, skl, etc.)
lsl r0, r4, #0x02
add r0, r0, r4
lsl r0, r0, #0x02
add r0, r0, r1
ldr r0, [ r0 ]
ldrb r0, [ r0, #0x0B ]
lsl r0, r0, #0x18
asr r0, r0, #0x18
blh GetUnit, r2 @ All this and before from vanilla routine
@ r0 = Character struct.
@ All I really care about here is checking if the unit is a phantom and putting in the summoner's character struct if necessary
ldr r1, [ r0, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID.
ldr r2, =PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
bne EndSummonProc
@ So it's a phantom. r0 still has the phantom's character struct.
bl FindSummoner @ r0 = summoner's character struct.

EndSummonProc:
mov r2, r0
ldr r1, =#0x0807ED4F
bx r1

@ I also need to fix the portrait in the level up screen.
	@ Hook at 0x0807F254
	@ Return to 0x0807F266

.global FixSummonPortrait
.type FixSummonPortrait, %function
FixSummonPortrait:
mov r0, #0x2E
ldsh r1, [ r5, r0 ]
lsl r0, r1, #0x02
add r0, r1
lsl r0, #0x02
add r0, r2
ldr r0, [ r0 ] @ From vanilla routine
@ r0 now has pointer to character struct.... or battle struct. It really doesn't matter.
ldr r1, [ r0, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r2, =#PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
bne EndSummonPortrait
@ So it's a phantom. Find the summoner's character struct and return the portrait ID in r1.
@ r0 still has the phantom's character struct.
bl FindSummoner

EndSummonPortrait:
ldr r0, [ r0 ]
ldrh r1, [ r0, #0x06 ] @ Return portrait ID in r1

mov r2, #0x32
ldr r0, =#0x0807F267
bx r0

@ Now to fix the text in the level up proc that shows the character's class.
	@ Text ID is called around 0x0807EB04
	@ Hook at 0x0807EAF4
	@ Return to 0x0807EB08

.global FixSummonClassText
.type FixSummonClassText, %function
FixSummonClassText:
ldr r0, =#0x02022CA8
add r4, r0
ldr r1, =#0x0203E1F0
lsl r0, r6, #0x02
add r0, r6
lsl r0, #0x02
add r7, r0, r1
ldr r0, [ r7 ]
@ r0 has the battle struct
ldr r1, [ r0, #0x04 ]
ldrb r1, [ r1, #0x04 ] @ Class ID
ldr r2, =PhantomIDSummonASM
ldrb r2, [ r2 ]
cmp r1, r2
bne EndClassText
@ So it's a phantom. Find the summoner's character struct and return the class name ID.
@ r0 still has the phantom's character struct.
bl FindSummoner @ r0 = Summoner's character struct.

EndClassText:
ldr r0, [ r0, #0x04 ]
ldrh r0, [ r0 ]

ldr r2, =#0x0807EB09
bx r2
