
.thumb

.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.macro Swap ra, rb
    eor \ra, \rb
    eor \rb, \ra
    eor \ra, \rb
.endm

.include "Functions.asm"
.include "SummonLevel.asm"
.include "PostBattle.asm"
.include "PostBattleAnimsOn.asm"
.equ Divide, 0x080D18FC
.equ GetUnit, 0x08019430

@ Start of EXP routine: 0x0802B92C

@ r4 = Defender's battle struct (203A56C)
@ r5 = Attacker's battle struct (203A4EC)
@ r6 = Current character's character struct?
@ r7 = ??? doesn't seem to do anything here

@ bl to 0x0802C534 seems to set r6 to attacker struct + EXP gained byte and determine EXP to be gained?
	@ bl to 0x0802BF94 seems to set EXP to 0 for a phantom within this one. This one checks if they have 0xFF experience, as in, they don't gain experience. This is the routine that checks if the unit can level up? Returns true and false in r0.

.global SummonEXPHack
.type SummonEXPHack, %function
SummonEXPHack:
ldr r0, [ r2, #0x04 ]
ldrb r0, [ r0, #0x04 ] @ Class ID
ldr r3, =PhantomIDSummonASM
ldrb r3, [ r3 ]
cmp r0, r3
beq EndEXPTrue

EndFalse:
ldrb r0, [ r2, #0x9 ]
cmp r0, #0xFF
bne SkipEXP
mov r0, #0x00
pop { r1 }
bx r1

SkipEXP:
mov r0, #0xB
ldr r1, =#0x0802BA0D
bx r1

EndEXPTrue:
push { r2 } @ Save the battle struct for later.
@ So this is a phantom. Make it able to gain EXP, but first, we need to make the phantom's EXP match the phantom's EXP for the EXP bar.
@ I need to find both character structs of the phantom and the summoner. First is the summoner so I can get the EXP value I need.
mov r0, r2
bl FindSummoner
pop { r2 }
cmp r0, #0x00
beq EndFalse @ No summoner was found. End.
@ r0 = Summoner's character struct, r2 = (phantom's) battle struct.
push { r0, r2 }
ldrb r0, [ r2, #0x0B ]
lsl r0, r0, #0x18
asr r0, r0, #0x18
blh GetUnit, r1 @ r0 = Phantom's character struct.
mov r1, r0
pop { r0, r2 }
ldrb r3, [ r0, #0x09 ] @ Summoner's current EXP.
strb r3, [ r1, #0x09 ] @ Store the summoner's EXP into the phantom's character struct.
strb r3, [ r2, #0x09 ] @ Put the summoner's EXP into the phantom's battle struct.
mov r1, #0x71
strb r3, [ r2, r1 ] @... and the EXP prior to battle byte.

@ Whew. Time to return valid to gain EXP.
mov r0, #0x01
pop { r1 }
bx r1

@ The phantom's EXP gets set to 0 at 0x0802C52A
