
;by EgalLau37/SageMatthis
;changes flag 0x80, field 1 to no longer force long-range animation and instead
;allows forcing a specific range type in field 4 using flags 01~10.
;currently, forcing melee distance from range softlocks the game, so it is
;disabled by default.
;free registers: r1, r2?
; SHORT 0x4901 0x8F46 0x6969 ; POIN ForceDistanceCode; WORD 0x75614C21 
.org 0x8051FB2
ldr r1,=ForceDistanceCode
mov r15,r1
.dh 0x6969

.pool
.dw 0x75614C21

ForceDistanceCode:
mov r2,r0
ldr r1,=0x01000000
and r2,r1
cmp r2,0x0
bne ForceMelee
mov r2,r0
ldr r1,=0x02000000
and r2,r1
cmp r2,0x0
bne ForceRanged
mov r2,r0
ldr r1,=0x04000000
and r2,r1
cmp r2,0x0
bne ForceLongRange
mov r2,r0
ldr r1,=0x08000000
and r2,r1
cmp r2,0x0
bne ForceSolo		;only included for fun, probably should not use for actual weapons
mov r2,r0
ldr r1,=0x10000000
and r2,r1
cmp r2,0x0
bne ForcePromotion	;only included for fun, probably should not use for actual weapons
ldr r1,=0x8051FE4
mov r15,r1

.pool

;cmp r1,0
;beq 0x8051FE4 ; fail

ForceMelee:
mov r0,0x0	; close-quarters
b EndCode	;
mov r0,0x1	; ranged
b EndCode
mov r0,0x2	; long-distance
b EndCode	;
mov r0,0x3	; solo
b EndCode	;
mov r0,0x4	; promotion

EndCode:
strh r0,[r4]
ldr r1,=0x8052066 ; 0x8051FEE
mov r15,r1

.pool
.close
