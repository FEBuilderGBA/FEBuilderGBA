
.thumb

Start:
mov     r3,#0x0
mov     r4,r7
Loop_Start:
add     r2,r4,r3
ldrb    r1,[r2]
ldr     r0,[r5,#0x4]
add     r0,#0x2C
add     r0,r0,r3
ldrb    r0,[r0]
cmp			r0,#0x0
bne		NormalAdd
mov			r1,#0x0
b		StoreRank
NormalAdd:
add     r1,r1,r0
cmp     r1,#0xFB
ble     StoreRank
mov     r1,#0xFB
StoreRank:
strb    r1,[r2]
add     r3,#0x1
cmp     r3,#0x7
ble     Loop_Start
b		End

.org 0x00000034
End:
mov			r1,#0x0
