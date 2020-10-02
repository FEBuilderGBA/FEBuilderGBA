@Call 2B438	{J}	//Hook r0
@Call 2B4E4	{U}	//Hook r0
.thumb

MOV r0, r8
LDRB r0, [r0, #0x13]
SUB r0, #0x1
STRH r0, [r5, #0x4]
LDR r3, [r4, #0x0]
LSL r0 ,r3 ,#0xD

LDR r1, =0x0802B440	@{J}
@LDR r1, =0x0802B4EC	@{U}
MOV PC, r1
