.align	2
.long	0x8000000

.set PROMO_HACK_ORG, 0x00000000
.set PROMO_HACK_SIZE, (End - Loop_Start + 6)

.align 2

.long	PROMO_HACK_ORG
.long	0x00000000
.long	PROMO_HACK_SIZE

.thumb
.align 2

Loop_Start:
@link from 298D8
@r14 = 298DD
add     r1,r3,r2
ldr     r0,[r4,#0x4]
add     r0,#0x2C
add     r0,r0,r2
ldrb    r0,[r0]
cmp		r0,#0x0
bne     Normal
b       Store_Rank
Normal:
ldrb    r5,[r1]
add     r0,r0,r5
cmp     r0,#0xFB
ble     Store_Rank
mov     r0,#0xFB
Store_Rank:
strb    r0,[r1]
add     r2,#0x1
cmp     r2,#0x7
ble     Loop_Start
ldr     r1, =0x080298f5
bx      r1
End:

@go back at 298f4
