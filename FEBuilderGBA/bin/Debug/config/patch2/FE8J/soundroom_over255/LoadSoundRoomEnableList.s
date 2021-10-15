.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
.macro blh_keep to
  push {r3}
  ldr r3, =\to
  mov lr, r3
  pop {r3}
  .short 0xf800
.endm

push {r4,lr}
mov r4 ,r0
blh 0x080a76f4   @IsSramWorking	@{J}
@blh 0x080a2cb0   @IsSramWorking	@{U}
lsl r0 ,r0 ,#0x18
cmp r0, #0x0
beq ReturnFalse

cmp r4, #0x0
bne LoadBuffer
ldr r4, =0x02020188 @gGenericBuffer	{J}
@ldr r4, =0x02020188 @gGenericBuffer	{U}

LoadBuffer:
ldr r1, =0x03006790 @gpReadSramFast	{J}
@ldr r1, =0x030067A0 @gpReadSramFast	{U}
ldr r0, =0x0E000000 @SaveDataArea
ldr r2, =0x080A88D8	@SaveData�ɒǉ������� {J}
@ldr r2, =0x080A3E94	@SaveData�ɒǉ������� {U}
ldr r2, [r2]
add r0 ,r0, r2      @�Z�[�u�f�[�^��SoundRoom��

ldr r3, [r1, #0x0]	@gpReadSramFast
mov r1 ,r4
mov r2, #0x80
blh_keep 0x080d65c8   @_call_via_r3	@{J}
@blh_keep 0x080d18cc   @_call_via_r3	@{U}

@���������̏ꍇ�A�S��0x00�ɂȂ��ۂ��̂ŁA������ƍl���܂�
@�Ō�̃f�[�^�͗]��������̂ŁA�ŏ�ʃr�b�g�𔻒�Ɏg���B
@�]���Ȃ̂�0�ɂȂ��Ă���̂�������
mov r0, r4
mov r1, #0x80 - 1
ldrb r0, [r4, r1]
mov r1, #0x80  @�ŏ�ʃr�b�g
and r0, r1
bne ReturnFalse

ReturnTrue:
mov r0, #0x1
b Exit

ReturnFalse:
mov r0 ,r4
mov r1 ,#0x0
mov r2 ,#0x80   @�r�b�g�t���O��1000����̈�����������܂�
blh 0x080D6968	@memset	{J}
@blh 0x080d1c6c		@memset	{U}

mov r0, #0x0

Exit:
pop {r4}
pop {r1}
bx r1
