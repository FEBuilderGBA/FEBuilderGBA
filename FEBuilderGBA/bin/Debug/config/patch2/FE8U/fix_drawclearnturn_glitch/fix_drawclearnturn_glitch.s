.thumb
@ORG @BC370	{J}
@ORG @B78CC	{U}
@Hook r0

ldr r0, [r3, #0x0]
lsl r0 ,r0 ,#0x19
lsr r0 ,r0 ,#0x19
ldr r1 , ENDCHAPTER
cmp r0 , r1
bge FalseExit

mov r10, r0
@ldr r1, =0x080BC378|1	@{J}
ldr r1, =0x080B78D4|1	@{U}
bx  r1

FalseExit:
@ldr r1, =0x80bc574|1	@{J}
ldr r1, =0x080b7b16|1	@{U}
bx  r1

.ltorg
ENDCHAPTER:
