

@hook: ORG 0x18418; jumpToHack(CasualMode)

.thumb
.org 0
CasualMode:
@check for something

push {r4}
mov r4,r2

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

cmp r3, #9
beq SkipClearSupports
ldr r3,=0x80283e1
bl goto_r3
SkipClearSupports:

pop {r4}
ldr r3,=0x801842d
goto_r3:
bx r3

CasualCheck:
push {lr}
ldr r0, =0x0202BD32 @ (ChapterData &01=Show objective (set=off, not set=on) &02=Combat animations off (if both 2 and 4 are set, combat animations are on with backgrounds on) &04=Combat animations solo (if both 2 and 4 are not set, combat animations are on with backgrounds off) &08=Combat info window (set=detail, not set=strat) &10=Combat info window (set=off, not set=strat))
ldrb r0, [r0]
lsl r0 ,r0 ,#0x1f
cmp r0, #0x0
beq CasualCheck_exit
mov r0, #0x1
CasualCheck_exit:
pop {pc}

