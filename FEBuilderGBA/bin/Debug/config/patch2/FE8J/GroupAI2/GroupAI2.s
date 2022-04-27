@called at 0803780C
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.equ ActionStruct, 0x0203A954
.equ Attacker, 0x0203A4E8
.equ Defender, 0x0203A568
.equ CurrentUnit, 0x03004DF0
.thumb
@r4 has attacker pointer in ram (actual character pointer, not attacker pointer)
@r5 has defender pointer in ram (actual character pointer, not defender pointer)
@r6 has action struct
mov r5, r0 @ vanilla 
ldr r4, =CurrentUnit 
push    {r4-r6,lr}
ldr r4, =Attacker
ldr r5, =Defender
ldr r6, =ActionStruct
@check if attacked this turn
ldrb     r0, [r6,#0x11]    @action taken this turn
cmp    r0, #0x2 @attack
bne    End

mov r0, #0x41
ldrb r6, [r4,r0]
mov r0, #0x5F
and r6, r0
cmp r6, #0x0
bne ActivateGroup

CheckDefender:
mov r0, #0x41
ldrb r6, [r5,r0]
mov r0, #0x1F
and r6, r0
cmp r6, #0x0
beq End

ActivateGroup:
mov r4, #0x80 @first enemy unit
ldr r5, =0x08019108 @get ram from dplynum
NextUnit:
mov r0, r4
mov lr, r5
.short 0xf800
@r0 is now ram

mov r2, #0x41
ldrb r3, [r0,r2]
mov r2, #0x1F
and r3, r2
cmp r3, r6
bne NotInGroup

mov r2, #0x41
ldrb r3, [r0,r2]
mov r2, #0xE0
and r3, r2
mov r2, #0x41
strb r3, [r0, r2]
mov r2, #0x44
mov r3, #0x0
strb r3, [r0,r2]

@add unit to the AI list so enemies act twice
ldr    r2,=0x203A9FF
ldrb    r1, [r0,#0x0B]    @allegiance byte of the character we are checking
AddAILoop:
add    r2, #0x01
ldrb    r3, [r2]
cmp    r3, #0x00
bne    AddAILoop
strb    r1, [r2]
add    r2, #0x01
strb    r3, [r2]

NotInGroup:
add r4, #1
cmp r4, #0xBF
blt NextUnit

End:
pop    {r4-r6}


ldr r0, [r4] 
blh 0x08018E64 @GetUnitCurrentHP 
pop {r3} @ useless lr I guess 
ldr r1, =0x08037818|1
bx r1 
.ltorg
.align 



