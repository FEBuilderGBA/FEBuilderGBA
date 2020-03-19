.thumb

.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.thumb
push {r4, r5, lr}
mov 	r4, r0
add 	r0, #0x64
ldrh 	r1, [r0, #0x0]
add 	r2, r1, #0x1
strh 	r2, [r0, #0x0]
lsl 	r1, r1, #0x10
asr		r1, r1, #0x10
mov 	r5, r1
cmp 	r1, #0x27
ble 	Continue
	mov 	r0, r4
	blh 	0x08002E94   	@Break6CLoop	{U}
@	blh 	0x08002DE4   	@Break6CLoop	{J}
Continue:
ldr 	r0, [r4, #0x50]
ldr 	r1, [r4, #0x2c]
ldr 	r2, [r4, #0x30]
mov		r3, #0x80
lsl 	r3, r3, #0x1
orr 	r2, r3
blh 	0x080092BC   		@TCS_Update	{U}
@blh 	0x080091AC   		@TCS_Update	{J}

@Added - call TCS_End to free AP space when finished
cmp 	r5, #0x27
ble 	End
	ldr 	r0, [r4, #0x50] @AP pointer
	blh 	0x080092A4 		@TCS_End	{U}
@	blh 	0x08009194 		@TCS_End	{J}

End:
pop {r4, r5}
pop {r0}
bx r0
