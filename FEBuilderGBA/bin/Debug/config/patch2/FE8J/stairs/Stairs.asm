
.thumb
.global StairsUsability
.type StairsUsability, %function

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

StairsUsability:
push { r4 - r6, lr }
@ First, I want to prevent someone using multiple stairs in one turn.
ldr r0, =#0x03004DF0	//
ldr r0, [ r0 ]
mov r1, #0x3A
ldrb r0, [ r0, r1 ] @ This should be 0 if no stairs were messed with this turn.
cmp r0, #0x00
bne EndFalseNoPop

ldr r0, =#0x0202BCEC @ Chapter data struct
ldrb r0, [ r0, #0x0E ] @ Chapter ID
blh #0x080345B8, r1 @ Pointer to chapter events in r0
ldr r2, [ r0, #0x08 ] @ Pointer to location events in r2
sub r2, #12

ldr r0, =#0x0203A954	//
ldrb r1, [ r0, #0x0F ] @ Y coordinate moving to in r1
ldrb r0, [ r0, #0x0E ] @ X coordinate moving to in r0

push { r2 }
BeginCheck1:
add r2, #12
ldrh r3, [ r2 ]
cmp r3, #0x00
beq EndFalse @ If this is an END_MAIN, end. No correct LOCAs exist.
ldrb r3, [ r2, #10 ] @ Command ID of first LOCA
cmp r3, #0x22
bne BeginCheck1 @ This doesn't have a 0x22 command. Try again.
ldrb r3, [ r2, #8 ] @ X coordinate of this LOCA
cmp r0, r3
bne BeginCheck1
ldrb r3, [ r2, #9 ] @ Y coordinate of this LOCA
cmp r1, r3
bne BeginCheck1
@ If it made it this far, then this is an applicable LOCA.

@ Now to check whether there's a unit on the other end of the stairs.
@ X coordinate in r5, Y coordinate in r6.
ldrb r4, [ r2, #2 ] @ Keep the relevant stair ID in r4
mov r5, r0
mov r6, r1

pop { r2 } @ Get begining of location events back into r2.
BeginCheck4:
add r2, #12
ldrb r3, [ r2, #10 ] @ Command ID of first LOCA
cmp r3, #0x22
bne BeginCheck4 @ This doesn't have a 0x22 command. Try again.
ldrb r3, [ r2, #2 ] @ This stair ID in r3
cmp r3, r4
bne BeginCheck4
@ Great. If I got this far, I have the pointer to the LOCA I'm looking for (except I still need to check that this isn't the LOCA that I'm currently at) I can get the coordinates to move to from here.
ldrb r0, [ r2, #8 ] @ X coordinate in r0
ldrb r1, [ r2, #9 ] @ Y coordinate in r1
cmp r0, r5
bne Skip2 @ If the X coordinates are different, then this isn't the same LOCA. Continue.
cmp r1, r6
beq BeginCheck4 @ If the X and Y coordinates are different, try again.

Skip2:
@ Now I need to check if there's a unit at these matching coordinates
ldr r2, =#0x0202BE48	//
sub r2, #0x48
StartCharacterLoop:
add r2, #0x48
ldr r3, [ r2 ]
cmp r3, #0x00 @ Check if this is the final entry in the character struct. If so, then no one's on that tile.
beq EndTrue
ldrb r3, [ r2, #0x10 ]
cmp r0, r3
bne StartCharacterLoop @ If not on the same X coordinate, loop back
ldrb r3, [ r2, #0x11 ]
cmp r1, r3
bne StartCharacterLoop

@ If it's here, then the X coordinate is the same along with the Y. There's a unit on the other end, so return grey.
@ EndGrey:

mov r0, #2
pop { r4 - r6 }
pop { r1 }
bx r1

EndTrue:
mov r0, #1
pop { r4 - r6 }
pop { r1 }
bx r1

EndFalse:
pop { r2 } @ Oopsie I broke the stack because I left my loop within a push / pop.
EndFalseNoPop:
mov r0, #3
pop { r4 - r6 }
pop { r1 }
bx r1

@345B8: (Get_Chapter_Events) (FE8U:346B0) (FE8J: 345B8) (FE7: 315BC)
@Params: r0=chapter number
@Returns: Pointer to that chapter's events

.global StairsEffect
.type StairsEffect, %function
StairsEffect:
push { r4 - r7, lr }
mov r7, r0
mov r5, r0 @ First to check if the "someone's on the other side" flag is set.
mov r4, r1
mov r0, r4
add r0, #0x3D
ldrb r0, [ r0 ]
cmp r0, #0x2
bne EffectCheck
ldr r1, =StairErrorTextLocation
ldrh r1, [ r1 ]
mov r0, r5
blh #0x080502F4, r2 @ Sets that text ID for the error text
mov r0, #0x08
b EndEffect

EffectCheck:
ldr r0, =#0x0202BCEC @ Chapter data struct
ldrb r0, [ r0, #0x0E ] @ Chapter ID
blh #0x080345B8, r1 @ Pointer to chapter events in r0
ldr r2, [ r0, #0x08 ] @ Pointer to location events in r2
sub r2, #12

ldr r0, =#0x0203A954	//
ldrb r1, [ r0, #0x0F ] @ Y coordinate moving to in r1
ldrb r0, [ r0, #0x0E ] @ X coordinate moving to in r0

push { r2 }
BeginCheck2:
add r2, #12
ldrb r3, [ r2, #10 ] @ Command ID of first LOCA
cmp r3, #0x22
bne BeginCheck2 @ This doesn't have a 0x22 command. Try again.
ldrb r3, [ r2, #8 ] @ X coordinate of this LOCA
cmp r0, r3
bne BeginCheck2
mov r5, r3 @ Store the X coordinate in r5 for later.
ldrb r3, [ r2, #9 ] @ Y coordinate of this LOCA
cmp r1, r3
bne BeginCheck2
mov r6, r3 @ Store the Y coordinate in r6 for later.
ldrb r4, [ r2, #2 ] @ Stair ID in r4

pop { r2 } @ Get the pointer to beginning of location events back
BeginCheck3:
add r2, #12
ldrb r3, [ r2, #10 ] @ Command ID of first LOCA
cmp r3, #0x22
bne BeginCheck3 @ This doesn't have a 0x22 command. Try again.
ldrb r3, [ r2, #2 ] @ This stair ID in r3
cmp r3, r4
bne BeginCheck3
@ Great. If I got this far, I have the pointer to the LOCA I'm looking for (except I still need to check that this isn't the LOCA that I'm currently at) I can get the coordinates to move to from here.
ldrb r0, [ r2, #8 ] @ X coordinate in r0
cmp r0, r5
beq SameX
ldrb r1, [ r2, #9 ] @ Y coordinate in r1
b Skip

SameX:
ldrb r1, [ r2, #9 ] @ Y coordinate in r1
cmp r1, r6
beq BeginCheck3

@ldr r2, =#0x03004DF0	//
@ldr r2, [ r2 ] @ Pointer to current character struct
@strb r0, [ r2, #0x10 ]
@strb r1, [ r2, #0x11 ]

Skip:
@push { r0, r1 }
ldr r4, =#0x03004DF0	//
ldr r4, [ r4 ]
@mov r0, r4
@blh #0x080280A0, r3 @ Stuff

@blh #0x0807B4B8, r3 @ Ends unit movement

@mov r0, r4
@blh #0x0807A888, r3 @ More movement stuff?

@mov r0, r7
@mov r1, #0x01
@blh #0x08002F24, r3
@pop { r0, r1 }
ldr r2, =#0x0203A954	//
strb r0, [ r2, #0x0E ]
strb r1, [ r2, #0x0F ] @ Sets new coordinates in the action struct
mov r3, #0x01
strb r3, [ r2, #0x11 ] @ Sets "wait"

strb r0, [ r4, #0x10 ]
strb r1, [ r4, #0x11 ]

ldrb r2, [ r2, #0x10 ] @ Has squares moved.
cmp r2, #0x00
bne SquaresMoved
mov r2, #0xFF

SquaresMoved:
mov r1, #0x01
lsl r1, r1, #7
orr r2, r1 @ Sets the top bit to mark that stairs are being used this turn.

mov r1, #0x3A
strb r2, [ r4, r1 ]

mov r0, #0x17

EndEffect:
pop { r4 - r7 }
pop { r3 }
bx r3

.global FixWait2
.type FixWait2, %function
FixWait2: @ Autohook to 0x080184AE	//
cmp r0, #0x00
beq EndWaitFix2 @ From vanilla routine
ldr r0, [ r6 ]

mov r1, #0x3A
ldrb r1, [ r0, r1 ]
cmp r1, #0x00
bne WeGotStairs

ldr r1, [ r0, #0x0C ]
mov r2, #0x02
neg r2, r2
and r1, r2
str r1, [ r0, #0x0C ]
b EndWaitFix2

WeGotStairs:
mov r1, #0x00
strb r1, [ r0, #0x0C ]

EndWaitFix2:
ldr r0, [ r6 ]
blh #0x080181B0, r1
pop { r4 - r6 }
pop { r0 }
bx r0

.global MoveDebuff
.type MoveDebuff, %function
MoveDebuff: @ Autohook to 0x0801C7D8	//
push { r4 - r6, lr }
mov r6, r0
mov r4, #0x01
ldr r5, =#0x03004DF0	//
ldr r0, [ r5 ]
ldr r1, [ r0, #0x04 ]
ldrb r1, [ r1, #0x12 ]
ldrb r2, [ r0, #0x1D ]
add r1, r2 @ All from vanilla. Basically calculates movement.

@ Now to add my debuff.
mov r3, #0x3A
ldrb r2, [ r0, r3 ]
lsl r2, r2, #25
lsr r2, r2, #25 @ Remove the stair flag... Won't be needing that here.
cmp r2, #0x7F @ 0x7F because the top bit has been unset?
beq ItsZero
sub r1, r2
ItsZero:
ldr r2, =#0x0203A954	//
ldr r3, =#0x0801C7EC+1
bx r3

.global EndTurnFix
.type EndTurnFix, %function
EndTurnFix:
push { lr }

@ Now to loop through the character structs and unset all 0x3A bytes.
ldr r0, =#0x0202BE48	//
mov r1, #0x00
mov r2, #0x3A
BeginEndTurnLoop:
strb r1, [ r0, r2 ]
add r0, #0x48
ldr r3, [ r0 ]
cmp r3, #0x00
bne BeginEndTurnLoop
@ If I'm here, I've run out of character entries.

ldr r0, =#0x085C2F58 @ From vanilla
blh #0x08002FC8, r1
mov r0, #0x17
pop { r1 }
bx r1

.global UnsetMoveDebuff
.type UnsetMoveDebuff, %function
UnsetMoveDebuff:
ldr r1, =#0x03004DF0	//
ldr r1, [ r1 ]
mov r3, #0x3A
ldrb r2, [ r1, r3 ] @ Movement debuff with the top bit set if stairs were taken.
lsr r3, r2, #7 @ r3 has the boolean on whether stairs were taken in this action.
cmp r3, #0x01
beq StairsTaken

@ If I'm here, they're NOT taking the stairs. Therefore, they're ending their turn. I need to completely unset the movement debuff.
mov r2, #0x00
mov r3, #0x3A
strb r2, [ r1, r3 ]
b EndUnsetDebuff

StairsTaken: @ If I'm here, they're taking the stairs. I need to unset the top bit and preserve the movement debuff.
lsl r2, r2, #25
lsr r2, r2, #25
mov r3, #0x3A
strb r2, [ r1, r3 ]
@b EndUnsetDebuff

EndUnsetDebuff:
pop { r4 - r5 }
pop { r1 }
bx r1

.global ActionPickRepoint
.type ActionPickRepoint, %function
ActionPickRepoint:
mov r0, r4
blh #0x0802FF04, r2 @ ActionPick
mov r0, #0x00
b UnsetMoveDebuff
