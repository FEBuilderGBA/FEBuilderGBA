
.thumb
.global WeaponTypeArray
.type WeaponTypeArray, %function

@ r0 has actor struct
@ r1 has target struct
@ this just replaces the entire WTA calc because there's not enough room to fit changes in place

WeaponTypeArray:
push {r4-r6,lr} @ vanilla
mov r4, r0 @ vanilla movement
mov r5, r1 @ vanilla movement
LDR r2, =#WeaponTriangleTable @ this is the weapon triangle table
LDR r6, =#WeaponTypeArrayList @ I am in love with this name. r6 was pushed AND is clobbered right after where we want to return
B Loop @ it's easy to miss when reading the disassembly that this is NOT the same address as the WTAloop branch

WTAloop:
MOV r0 ,r4 @ actor struct! so obviously next is
ADD r0, #0x4A @ weapon type. 0x4A is the most useful weapon ID for our purposes
LDRB r1, [r0, #0x0] @ loads that into r1
MOV r0, #0x0 @ then zeroes r0
add r0, r1, r6 @ r0 now has position of weapon type
ldrb r1, [r0, #0x0] @ now we put that byte in r1
mov r0, #0x0 @ start with 0, end with 0
LDSB r0, [r2, r0] @ because that zero loads our place in wta chart
CMP r1 ,r0 @ Is this the right first part?
BNE IncAndLoop @ If not, increment and loop
MOV r0, r5 @ so this must be target struct
ADD r0, #0x4A @ and so do the same 
LDRB r1, [r0, #0x0] @ We don't ever need both actor and target at the same time for this, nice
add r0, r1, r6 @ same as before
ldrb r1, [r0, #0x0] @ put the byte in r1
MOV r0, #0x1 @ load 1 into r0 because signed load, works for me because it clobbers
LDSB r0, [r2, r0] @ Why is this load signed?
CMP r1 ,r0 @ Is this the right second weapon for this entry?
BNE IncAndLoop @ If not increment and loop
LDRB r0, [r2, #0x2] @ We can leave this all alone because this just applies the effectiveness, this loads in the hit bonus
MOV r1 ,r4 @ AND it clobbers r1 that we left dirty with actor struct
ADD r1, #0x53 @ add position of hit bonus 
STRB r0, [r1, #0x0] @ store it
LDRB r1, [r2, #0x3] @ pointer:0859BA93 -> 0F0200FF load in damage bonus 
MOV r0 ,r4 @ actor struct in r0
ADD r0, #0x54 @ dmg bonus 
STRB r1, [r0, #0x0] @ store it
LDRB r0, [r2, #0x4] @ target hit bonus
MOV r1 ,r5 @ target struct
ADD r1, #0x53 @ hit bonus slot
STRB r0, [r1, #0x0] @ store it 
LDRB r0, [r2, #0x5] @ target damage bonus
ADD r1, #0x1 @ Why does this one do something different for moving in table?
STRB r0, [r1, #0x0] @ well, store in slot 53+1
B Cleanup @ we're done with WTA calc!

IncAndLoop:
ADD r2, #0x6 @ forward a spot in wta chart
Loop:
MOV r0, #0x0 @ clean
LDRB r0, [r2, r0] @ pointer:0859BA94 -> 010F0200 ????? this suddenly broke when someone else looked at it, and started working when I changed this to unsigned
CMP r0, #0xFF @ check if we hit a terminator
BNE WTAloop @ if not, keep going!
@ If it is, we're completely done!

Cleanup:
LDR r6, =0x802C81B @ r4 and r5 are expected to remain the same for the rest of this subcommand, and we didn't touch them, r6 is clobbered right after we return, Vanilla clobbers r0, r1 is target struct like in vanilla, r2 still has the position in effectiveness for reverse calcs, and r3 is untouched, so nothing should really change
BX r6 @ sorry you made the trip for nothing
