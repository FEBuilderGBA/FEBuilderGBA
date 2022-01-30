.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@Hook1 080B48E8	@{J}
@Hook2 080B4A88	@{J}
@Hook1 080AFCD0	@{U}
@Hook2 080AFE60	@{U}

CheckA:
mov r0, #0x1
and r0 ,r1
cmp r0, #0x0
bne RandomBG

CheckB:
mov r0, #0x2
and r0 ,r1
cmp r0, #0x0
bne ExitRandomMode

CheckStart:
mov r0, #0x4
and r0 ,r1
cmp r0, #0x0
bne ExitRandomMode

CheckSelect:
mov r0, #0x4
and r0 ,r1
cmp r0, #0x0
bne ExitRandomMode

CheckUp:
mov r0, #0x40
and r0 ,r1
cmp r0, #0x0
bne RandomBG

CheckDown:
mov r0, #0x80
and r0 ,r1
cmp r0, #0x0
bne RandomBG

b Exit

RandomBG:
	@汚い方法を取る。
	@背景切替を行っているフックの個所へ無理やり飛ばします
	push {r4,r5,r6}
@	ldr r1, =0x080B4458|1			@{J}
	ldr r1, =0x080AF838|1			@{U}
	bx r1

ExitRandomMode:
	mov r0 ,r3
@	blh 0x08002de4   @Break6CLoop	{J}
	blh 0x08002e94   @Break6CLoop	{U}

Exit:
pop {r0}
bx r0
