.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

ArenaDicCommandEffect:
push {lr}
ldr r0, ArenaDicProcs
mov r1, #0x3
blh 0x08002bcc   @New6C	{J}

mov  r1, #0x0
strh r1, [r0, #0x2A]	@thisProcs->CurrentPage
strh r1, [r0, #0x2C]	@thisProcs->AllCount
strh r1, [r0, #0x2E]	@thisProcs->ComplateCount

mov r0, #0x17
pop {r1}
bx r1

.ltorg
ArenaDicProcs:
