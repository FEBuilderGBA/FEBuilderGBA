@Call 08028234	{J}
@Call 080282A0	{U}
.thumb



@壊す値の再送
ldrb r6, [r0, #0x0]	@SupportGain
mov r0 ,r2
add r0, #0x32
add r7 ,r0, r1

@keep r2 !!

@ldr r4,  =0x0202BCEC @ChapterData	{J}
ldr r4,  =0x0202BCF0 @ChapterData	{U}
ldrb r4, [r4, #0xe]  @MapID

ldr r3, Table
sub r3, #0x2

Loop:
add r3, #0x02
ldrh r0, [r3]
cmp  r0, #0x0
beq  Exit

ldrb r0, [r3]
cmp  r0, r4   @CheckMapID
bne  Loop

Found:
ldrb r0, [r3, #0x01]
mul r6,r0  @SupportGain * scale
@b    Exit

Exit:
@ldr r3, =0x0802823C|1	@{J}
ldr r3, =0x080282A8|1	@{U}
bx r3

.ltorg
.align
Table:
