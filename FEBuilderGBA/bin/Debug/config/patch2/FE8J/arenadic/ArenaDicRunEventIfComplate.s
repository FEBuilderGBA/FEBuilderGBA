.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blh_ to, reg=r3
  push {\reg}
  ldr \reg, =\to
  mov lr, \reg
  pop {\reg}
  .short 0xf800
.endm

@�R���v���[�g�C�x���g
ComplateEvent:
push {r4,lr}
mov  r4, r0	@this procs

@����
ldrh r0, [r4, #0x2C]	@thisProcs->AllCount

@�B����
ldrh r1, [r4, #0x2E]	@thisProcs->ComplateCount

cmp r0, r1
bne ComplateEvent_Exit	@�R���v���[�g���Ă��Ȃ��Ȃ�{�c

ldr r3, ArenaDicConfig
ldrh r0, [r3, #0x8]	@ArenaDicConfig->�S�B�����̒B���t���O
cmp  r0, #0x0
beq  ComplateEvent_Exit	@�B���t���O0���Ɩ���N���Ă��܂��̂͂悭�Ȃ��̂ŁA�{�c�ɂ���

blh 0x080860D0	@CheckFlag
cmp r0, #0x0
bne ComplateEvent_Exit	@���ɔ����ς݂Ȃ̂Ń{�c

@�t���O��L���ɂ��ăC�x���g���Ăяo��
ldr r3, ArenaDicConfig
ldrh r0, [r3, #0x8]	@ArenaDicConfig->�S�B�����̒B���t���O
blh  0x080860A8	@SetFlag

ldr r3, ArenaDicConfig
ldr r0, [r3, #0xC]	@ArenaDicConfig->�S�B�����̃C�x���g
cmp r0, #0x1
ble ComplateEvent_Exit	@�C�x���g�A�h���X������

mov r1, #0x2
blh 0x0800d340   @�C�x���g���߂𓮍삳����֐�	{J}
@blh 0x0800d07c   @�C�x���g���߂𓮍삳����֐�	{U}

ComplateEvent_Exit:
pop {r4}
pop {r0}
bx r0



.ltorg
DATA:
.equ	ArenaDicConfig,	DATA+0
