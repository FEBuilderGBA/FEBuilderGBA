.thumb
.align

.macro blh to, reg=r3
    ldr \reg, =\to
    mov lr, \reg
    .short 0xF800
.endm
.equ ProcGoto,0x8002F24

@just spent 6 weeks doing the wrong thing here :)

@class to promote to is r4 +0x3B
@current unit is r2 + 0x2C (which is pointer at [r4 + 0x18] + 0x2C)



@r0 = proc data

.global SplitPromoItems
.type SplitPromoItems, %function
SplitPromoItems:

push {r4-r7}

@unnecessary part checking if there is only one promo option:
@mov r0,r1
@add r0,#0x34
@ldrb r0,[r0]
@cmp r0,#0 
@bne CC616

@set class to promote to
@mov r0,r1
@add r0,#0x35
@ldrb r0,[r0]
@mov r1,r2
@add r1,#0x3B
@strb r0,[r1]

mov r6,r1
mov r7,r2

@action struct has the slot # our promo item is in
ldr r0,=0x203A958 @action struct
add r0,#0x12
ldrb r0,[r0]
lsl r0,r0,#1

ldr r1,[r2,#0x2C] @unit struct
add r1,#0x1E

add r1,r0
ldrh r0,[r1] @r0 = item & uses
mov r1,#0xFF
and r0,r1 @r0 = item ID


@now we check this against our external list
ldr r1,=SplitPromoItemsList
SplitPromoItemListCheckStart:
ldrb r2,[r1]
cmp r2,#0
beq CheckItemList
cmp r0,r2
beq UsingSplitPromos
add r1,#1
b SplitPromoItemListCheckStart

CheckItemList:
ldr r1,=PromoItemsList
PromoItemListCheckStart:
ldrb r2,[r1]
cmp r2,#0
beq GoBack @this shouldn't be able to happen but case nonetheless
cmp r0,r2
beq GetPromoList
add r1,#8
b PromoItemListCheckStart

GetPromoList:
add r1,#4
ldr r0,[r1] @r0 = offset of promo list

ldr r1,[r7,#0x2C] @unit struct
ldr r1,[r1,#4] @class data
ldrb r1,[r1,#4] @class ID

PromoListCheckStart:
ldrb r2,[r0]
cmp r2,#0
beq GoBack @this only happens through user error
cmp r2,r1
beq SetPromoClass
add r0,#2
b PromoListCheckStart

SetPromoClass:
ldrb r2,[r0,#1]
mov r1,r4
add r1,#0x3B
strb r2,[r1]

GoBack:
@update proc position
mov r0,r7
mov r1,#5
blh ProcGoto,r2

mov r1,r6
mov r2,r7
pop {r4-r7}
ldr r0,=0x80CC623 @our return point
bx r0

UsingSplitPromos:
mov r1,r6
mov r2,r7
pop {r4-r7}
ldr r0,=0x80CC61B
bx r0

.ltorg
.align

