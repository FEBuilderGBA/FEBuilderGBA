.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

CustomDeadMotion_Smash_ASM:
@08079C04 {J}
push {r4,r5,lr}
mov r4 ,r0
ldrh r0, [r4, #0x2c]
add r0, #0x1
strh r0, [r4, #0x2c]
lsl r0 ,r0 ,#0x10
asr r0 ,r0 ,#0x10

cmp r0, #0xA
ble Back

cmp r0, #0xB
beq StartAplha

cmp r0, #0x16
ble Up

cmp r0, #0x30
ble Down

Break:
	@no wait

	mov  r0, #0x0
	strh r0, [r4, #0x2c]
	strh r0, [r4, #0x30]
	mov r0 ,r4
	blh 0x08002de4  @Break6CLoop	{J}
@	blh 0x08002E94  @Break6CLoop	{U}
	b   Exit

StartAplha:
    ldr r0, [r4, #0x5c]
    ldr r1, [r4, #0x60]
	blh 0x08053e10   @StartEfxDeadPika	{J}
@	blh 0x08053120   @StartEfxDeadPika	{U}
	
	ldr r1, [r4, #0x5c]
	str r1, [r0, #0x5c]

	ldr r1, [r4, #0x60]
	str r1, [r0, #0x60]

	mov r1, #0x0
	strh r1, [r0, #0x2c]
	strh r1, [r0, #0x2e]
    b   Exit

Back:
	ldr r0, [r4, #0x5c]
	ldrh r1, [r0, #0x2]	@xPosition
	sub  r1, #0xA
	strh r1, [r0, #0x2]	@xPosition

	ldrh r1, [r0, #0x4]	@yPosition
	sub  r1, #0x1
	strh r1, [r0, #0x4]	@yPosition
	b   Exit

Up:
	ldr r0, [r4, #0x5c]
	ldrh r1, [r0, #0x2]	@xPosition
	add  r1, #0x8
	strh r1, [r0, #0x2]	@xPosition

	ldrh r1, [r0, #0x4]	@yPosition
	sub  r1, #0x5
	strh r1, [r0, #0x4]	@yPosition
	b   Exit

Down:
	ldr r0, [r4, #0x5c]

	ldrh r1, [r0, #0x2]	@xPosition
	sub  r1, #0x3
	strh r1, [r0, #0x2]	@xPosition

	ldrh r1, [r0, #0x4]	@yPosition
	add  r1, #0xA
	strh r1, [r0, #0x4]	@yPosition
	b   Exit


Flush:
	b   Exit

Exit:
pop {r4,r5}
pop {r0}
bx r0
