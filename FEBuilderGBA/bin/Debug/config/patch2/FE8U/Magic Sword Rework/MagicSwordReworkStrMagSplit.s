.thumb
.align 4

@hook at 2A882; r0 will contain item ID

@structure of list data:
@0000 0000  0000         0000                                            0000                 0000
@Item ID    Weapon type  Whether or not to use weapon type at any range  behavior at 1-range  behavior at 2-range

push {r2-r4}

@load magic sword list into r1

ldr r1,MagicSwordList

@cycle through list until received item ID is 0

LoopStart:
ldrb r2,[r1]
cmp r2,#0
beq GoBack
cmp r2,r0
beq ExitLoop
add r1,#3
b LoopStart

@to reach this point r1=list at the offset of the 3 bytes to use, r0=current item ID, r2=current item ID

ExitLoop:
ldrb r3,[r1,#1] @load weapon type byte into r3
ldrb r4,[r1,#2] @load range behavior byte into r4



@check range
ldrb r0,[r7,#2] @r7 contains battle stats pointer
cmp r0,#2
blt AtMelee
b AtRange

AtMelee:
lsr r4,r4,#4

mov r0,#0x1
tst r3,r0
bne AtRange

b TargetBehavior



AtRange:
mov r2,#0x3A
ldrb r0,[r5,r2]
strb r0,[r5,#0x14]

mov r0,#0x1
tst r3,r0
bne SetWeaponType

lsl r4,r4,#28
lsr r4,r4,#28

b SetWeaponType

SetWeaponType:
mov r0,r3
lsr r0,r0,#4
strb r0,[r6] @r6 contains attacker struct pointer + 0x50
b TargetBehavior

TargetBehavior: @0=hit def, 1=hit res, 2=hit lower stat, 3=hit higher stat
cmp r4,#0
beq HitDef

mov r0,r6
add r0,#0x30 @get defender struct
ldrb r1,[r0,#0x18] @enemy res
ldrb r0,[r0,#0x17] @enemy def
cmp r4,#2
beq HitLower
cmp r4,#3
beq HitHigher
b GoBack

HitLower:
cmp r0,r1
blt HitDef
b GoBack

HitHigher:
cmp r0,r1
bgt HitDef
b GoBack

HitDef:
ldr r0,[r5,#0x4C] @r5 contains attacker struct pointer
mov r1,#0x41
neg r1,r1
and r0,r1 @removes the bit for hitting magic damage
str r0,[r5,#0x4C]
b GoBackDef

GoBack:
pop {r2-r4}

ldr r0,ReturnPoint
mov	lr,r0
bx lr

GoBackDef:
pop {r2-r4}

ldr r0,DefReturnPoint
mov lr,r0
bx lr

.ltorg
.align 4

BattleStats:
.word 0x203A4D4
ReturnPoint:
.word 0x802A8B9
DefReturnPoint:
.word 0x802A8BD
MagicSwordList:
@POIN MagicSwordList
