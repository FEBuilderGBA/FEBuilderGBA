.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm


ASMC_ClearAllWorldMapPathAndNode:
	push {r4,lr}

@	blh 0x80C165C	@GmInit	{J}
	blh 0x80BC81C	@GmInit	{U}

@	ldr r4, =0x03005270	@WMEventRelatedStruct	{J}
	ldr r4, =0x03005290	@WMEventRelatedStruct	{U}

	mov r0, #0x34
	strb r0, [r4, #0x0]
	mov r0, #0x20
	strb r0, [r4, #0x02]
	mov r0, #0x8
	strb r0, [r4, #0x04]
	mov r0, #0x1
	strb r0, [r4, #0x12]

Exit:
	pop  {r4}
	pop  {r0}
	bx   r0

