
;by EgalLau37/SageMatthis

;splits flag 80, field 1 ability into two abilities
;flag 80, field 1 remains uncounterable while flag 20, field 4 becomes always decrement durability even on miss
;mostly applicable to weapons since magic always decrements durability by default

;free registers: r1, r2, r5?, r9?,r10?
.org 0x80294A4
ldr r1,=SplitDeductDurability
mov r15,r1
SHORT 0x6969

.pool

SplitDeductDurability:
ldr r0,[r5,0x4C]
ldr r2,[r5,0x4C]
mov r1,0x2
and r2,r1
cmp r2,0x0
bne HasFlag02Field_1
mov r1,0x20
lsl r1,r1,0x18
and r0,r1
cmp r0,0x0
beq HasNeitherFlag
HasFlag02Field_1:
ldr r1,=0x80294AE
mov r15,r1
.pool

HasNeitherFlag:
ldr r1,=0x80294C8
mov r15,r1
.pool


.close