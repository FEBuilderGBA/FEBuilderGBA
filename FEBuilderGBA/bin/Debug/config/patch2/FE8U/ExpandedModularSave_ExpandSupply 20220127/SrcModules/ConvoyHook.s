.thumb

.global MS_LoadConvoyItems_1
.type MS_LoadConvoyItems_1, %function
.global MS_LoadConvoyItems_2
.type MS_LoadConvoyItems_2, %function
.global MS_SaveConvoyItems_1
.type MS_SaveConvoyItems_1, %function
.global MS_SaveConvoyItems_2
.type MS_SaveConvoyItems_2, %function

@r0 has our offset and r1 has our size

MS_LoadConvoyItems_1:
	push 	{r4-r7, lr}

	@Offset the stack pointer by chunk size in r1
	mov 	r11, r1

	mov 	r2, sp
	sub 	r2, r1
	mov 	sp, r2

	mov 	r2, r1
	ldr 	r1, =0x030067A0
	ldr 	r3, [r1]
	mov 	r1, sp
	bl 		BXR3
	ldr 	r0, =GetConvoyItemArrayStart
	mov 	lr, r0
	.short 	0xf800
	mov 	r4, r0
	add 	r5, sp, #0x64
	add 	r5, #0x64		@200 expansion
	ldr		r1, =0x80A3284
	mov		pc, r1

BXR3:
	bx 		r3

.align
.ltorg

MS_LoadConvoyItems_2:
	@get chunk size from r11
	mov 	r1, r11
	mov 	r0, sp
	add 	r0, r1
	mov 	sp, r0

	pop 	{r4-r7}
	pop 	{r0}
	bx 		r0

	.align
	.ltorg

MS_SaveConvoyItems_1:
	push 	{r4-r7, lr}
	mov		r7,r8
	push	{r7}
	
	@Offset the stack pointer by chunk size in r1
	mov 	r11, r1
	
	mov 	r3, sp
	sub 	r3, r1
	mov 	sp, r3
	
	mov 	r8, r0
	ldr 	r0, =GetConvoyItemArrayStart
	mov 	lr, r0
	.short 	0xf800
	mov 	r6, r0
	add 	r5, sp, #0x64
	add 	r5, #0x64		@200 expansion
	ldr		r0, =0x80A31F6
	mov		pc, r0
	
	.align
	.ltorg
	
MS_SaveConvoyItems_2:
	mov 	r0, sp
	mov 	r1, r8
	mov 	r2, r11
	ldr 	r3, =0x080D184C
	mov 	r14, r3
	.short 	0xf800
	mov 	r1, r11
	mov 	r0, sp
	add 	r0, r1
	mov 	sp, r0
	pop		{r3}
	mov 	r8, r3
	pop 	{r4-r7}
	pop 	{r0}
	bx 		r0

	.align
	.ltorg
