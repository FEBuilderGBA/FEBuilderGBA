.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

CustomDeadMotion_Flush_And_Pika_ASM:
@08079C04 {J}
push {r4,r5,lr}
mov r4 ,r0
ldrh r0, [r4, #0x2c]
add r0, #0x1
strh r0, [r4, #0x2c]
lsl r0 ,r0 ,#0x10
asr r0 ,r0 ,#0x10
cmp r0, #0x1
beq Flush
cmp r0, #0x23
beq Flush
cmp r0, #0x32
beq Flush
cmp r0, #0x64
bne Exit

Break:
	ldr r0, [r4, #0x5c]
	ldr r1, [r4, #0x60]
@	blh 0x08053e10   @StartEfxDeadPika	{J}
	blh 0x08053120   @StartEfxDeadPika	{U}
	
	ldr r1, [r4, #0x5c]
	str r1, [r0, #0x5c]

	ldr r1, [r4, #0x60]
	str r1, [r0, #0x60]

	mov r1, #0x0
	strh r1, [r0, #0x2c]
	strh r1, [r0, #0x2e]

	mov  r0, #0x0
	strh r0, [r4, #0x2c]
	strh r0, [r4, #0x30]

	mov r0 ,r4
@	blh 0x08002de4  @Break6CLoop	{J}
	blh 0x08002E94  @Break6CLoop	{U}
	b   Exit

Flush:
	mov r0, #0x3
	mov r1, #0x2
	mov r2, #0x3
@	blh 0x08079ed0	@Start_ekrWhiteINOUT	{J}
	blh 0x08070568	@Start_ekrWhiteINOUT	{U}
	ldr r0, =0x147
	mov r1, #0x80
	lsl r1 ,r1 ,#0x1
	mov r2, #0x78
	mov r3, #0x0
@	blh 0x08074e80,r5   @効果音を鳴らすルーチン2 r0=効果音:SONG r1=? r2=? r3=?	{J}
	blh 0x080729A4,r5   @効果音を鳴らすルーチン2 r0=効果音:SONG r1=? r2=? r3=?	{U}
@	b   Exit

Exit:
pop {r4,r5}
pop {r0}
bx r0
