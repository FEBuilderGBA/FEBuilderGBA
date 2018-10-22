
.macro blh to, reg
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm

.thumb
.global AnimaTriangle
.type   AnimaTriangle, %function

AnimaTriangle:
beq VanillaLabel
mov r0, r4
mov r1, r5
ldr r5, =#0x802C76C
mov lr, r5
.short 0xF800
VanillaLabel:
pop { r4 - r6 } @ All from vanilla routine

push { r0 - r5 }
mov r2, #0
mov r3, #0
ldr r0, =#0x203A4EC
blh 0x08016B28, r2 @ Equiped weapon of the attacker in r0
lsl r0, r0, #24
lsr r0, r0, #24
push { r0 }
@add r0, #0x48
@ldrb r0, [ r0 ] @ Get attacker's item in r0

ldr r0, =#0x203A56C
blh 0x08016B28, r2 @ Equiped weapon of the defender in r1
lsl r0, r0, #24
lsr r1, r0, #24
pop { r0 }
@add r1, #0x48
@ldrb r1, [ r1 ] @ Get defender's item in r1

cmp r0, #0x00
beq End @ If the attacker has no weapon, end.
cmp r1, #0x00
beq End @ If the defender has no weapon, end.
ldr r5, =#FireList
FireLoopA:
ldrb r4, [ r5 ] @ Fire item in r4
cmp r0, r4
beq FireA @ If equal, it's a fire tome
cmp r4, #0x00
beq NotFireA @ If terminator, it's not fire. Go to thunder check
add r5, #1 @ Get pointer to next item in r0
b FireLoopA

FireA:
mov r2, #1	@ FIRE
b DefenderCheck

NotFireA:
ldr r5, =#ThunderList
ThunderLoopA:
ldrb r4, [ r5 ] @ Thunder item in r4
cmp r0, r4
beq ThunderA @ If equal, it's a thunder tome
cmp r4, #0x00
beq NotThunderA @ If terminator, it's not thunder. Go to wind check
add r5, #1
b ThunderLoopA

ThunderA:
mov r2, #2 @ THUNDER
b DefenderCheck

NotThunderA:
ldr r5, =#WindList
WindLoopA:
ldrb r4, [ r5 ] @ Wind item in r4
cmp r0, r4
beq WindA
cmp r4, #0x00
beq End @ If it made it through this, attacker's weapon is not fire, thunder, or wind, so it's not applicable.
add r5, #1
b WindLoopA

WindA:
mov r2, #3 @ WIND

DefenderCheck:
ldr r5, =#FireList
FireLoopD:
ldrb r4, [ r5 ] @ Fire item in r4
cmp r1, r4
beq FireD @ If equal, it's a fire tome
cmp r4, #0x00
beq NotFireD @ If terminator, it's not fire. Go to thunder check
add r5, #1 @ Get pointer to next item in r0
b FireLoopD

FireD:
mov r3, #1	@ FIRE
b TriangleStuff

NotFireD:
ldr r5, =#ThunderList
ThunderLoopD:
ldrb r4, [ r5 ] @ Thunder item in r4
cmp r1, r4
beq ThunderD @ If equal, it's a thunder tome
cmp r4, #0x00
beq NotThunderD @ If terminator, it's not thunder. Go to wind check
add r5, #1
b ThunderLoopD

ThunderD:
mov r3, #2 @ THUNDER
b TriangleStuff


NotThunderD:
ldr r5, =#WindList
WindLoopD:
ldrb r4, [ r5 ] @ Wind item in r4
cmp r1, r4
beq WindD
cmp r4, #0x00
beq End @ If it made it through this, defender's weapon is not fire, thunder, or wind, so it's not applicable.
add r5, #1
b WindLoopD

WindD:
mov r3, #3 @ WIND

TriangleStuff:

@ Now for checking triangle advantages
cmp r2, r3
beq End	@ If they're the same, I don't care.

cmp r2, #1
beq FireAttack
cmp r2, #2
beq ThunderAttack
b WindAttack @ So it has to be wind

FireAttack:
cmp r3, #2
beq SetDefenderAdvantage @ Then it's thunder
b SetAttackerAdvantage @ Then it's wind

ThunderAttack:
cmp r3, #3
beq SetDefenderAdvantage @ Then it's wind
b SetAttackerAdvantage @ Then it's fire

WindAttack:
cmp r3, #1
beq SetDefenderAdvantage @ Then it's fire
b SetAttackerAdvantage @ Then it's thunder


SetAttackerAdvantage:
ldr r0, =0x203A53F @ Location for hit change for attacker
ldr r1, =0x203A5BF @ Location for hit change for defender
mov r2, #0x0A      @ 10 for hit advantage
mov r3, #0xF6      @ -10 for hit disadvantage
strb r2, [ r0 ]
strb r3, [ r1 ]
mov r2, #0x01      @ 1 for damage advantage
mov r3, #0xFF      @ -1 for damage disadvantage
strb r2, [ r0, #1 ]
strb r3, [ r1, #1 ] @ Damage location is only one up from hit location
b End

SetDefenderAdvantage:
ldr r0, =0x203A53F @ Location for hit change for attacker
ldr r1, =0x203A5BF @ Location for hit change for defender
mov r2, #0x0A      @ 10 for hit advantage
mov r3, #0xF6      @ -10 for hit disadvantage
strb r3, [ r0 ]
strb r2, [ r1 ]
mov r2, #0x01      @ 1 for damage advantage
mov r3, #0xFF      @ -1 for damage disadvantage
strb r3, [ r0, #1 ]
strb r2, [ r1, #1 ] @ Damage location is only one up from hit location


End:
pop { r0 - r5 }
pop { r0 }
bx r0
