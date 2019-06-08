
.thumb
.type CopyStruct, %function
CopyStruct:
mov r3, #0x00 @ r3 is a counter
ContinueStructCopy:
ldr r2, [ r0, r3 ]
str r2, [ r1, r3 ]
add r3, #0x04
cmp r3, #0x48
bne ContinueStructCopy
bx lr

.type FindCharacter, %function
FindCharacter: @ r0 = character to find. Returns character struct.
push { r4 }
mov r4, r0
ldr r0, =#0x0202BE4C
sub r0, #0x48
StartFindCharacterLoop:
add r0, #0x48
ldr r1, [ r0 ]
cmp r1, #0x00
beq FindCharacterNoCharacter
ldrb r1, [ r1, #0x04 ] @ Character number.
cmp r1, r4
bne StartFindCharacterLoop
pop { r4 }
bx lr
FindCharacterNoCharacter:
mov r0, #0x00
pop { r4 }
bx lr

.type FindSummoner, %function
FindSummoner: @ r0 = phantom character struct. Returns the character struct of the summoner.
push { lr }
ldr r0, [ r0 ]
ldrb r0, [ r0, #0x04 ] @ Phantom's character ID.
ldr r1, =SummonCharacterTable
mov r2, #0x01 @ r2 is a counter.
FindSummonerLoop:
ldrb r3, [ r1, r2 ]
cmp r3, #0x00
beq FindSummonerNoSummoner
add r2, #0x02
cmp r0, r3
bne FindSummonerLoop
@ If we're here, this entry has the summoner's character ID.
sub r2, r2, #0x03
ldrb r0, [ r1, r2 ] @ Summoner's character ID.
bl FindCharacter @ r0 = summoner's character struct.
EndFindSummoner:
pop { r1 }
bx r1
FindSummonerNoSummoner: @ No summoner was found. Let's just return 0.
mov r0, #0x00
b EndFindSummoner
