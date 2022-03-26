.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ASMC_CancelPostGame:
	push {lr}

@	ldr  r3, =0x030004B0	@MemorySlot	{J}
	ldr  r3, =0x030004B8	@MemorySlot	{U}
	ldr  r0, [r3, #4*0x01] @slot 1 as unit 

	cmp  r0, #0x1
	beq  ON
OFF:
@	ldr r3,  =0x0202BCEC @ChapterData	{J}
	ldr r3,  =0x0202BCF0 @ChapterData	{U}

	mov  r0, #0x0
	strb r0, [r3, #0x14]

	add  r3, #0x4A
	ldrb r0, [r3]
	cmp  r0, #0x7
	bne  Exit

	mov  r0, #0x5 @stage
	strb r0, [r3]
	b    Exit

ON:
@	ldr r3,  =0x0202BCEC @ChapterData	{J}
	ldr r3,  =0x0202BCF0 @ChapterData	{U}

	ldrb r0, [r3, #0x14]
	mov  r1, #0x20
	orr  r0, r1
	strb r0, [r3, #0x14]

	mov  r0, #0x7 @POSTGME
	add  r3, #0x4A
	strb r0, [r3]
@	b    Exit

Exit:
	pop  {r0}
	bx   r0

