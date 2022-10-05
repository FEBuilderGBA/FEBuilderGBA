

@hook: ORG 0x17BEC; jumpToHack(CasualMode)

.thumb
.org 0
CasualMode:
@check for something

push {r4}
mov r4,r1

bl CasualCheck

cmp r0, #0
bne Retreat
mov r3, #5
b Main
Retreat:
mov r3,#9

@original code:
Main:
ldr r0,[r4,#0xc]
mov r1,#5
mvn r1,r1
and r0,r1 @unkill unit (needed for pair up compatibility)
mov r1,r3 @5 for dead, 9 for retreated
orr r0,r1
str r0,[r4,#0xc]
mov r0,r4
ldr r3,=0x08022C61
bl goto_r3
pop {r4}
ldr r3,=0x08017BFF
goto_r3:
bx r3

CasualCheck:
push {lr}
mov r0, #0x8C @unused permanent event id
ldr r3,=0x0806BA5D
bl goto_r3
pop {pc}

