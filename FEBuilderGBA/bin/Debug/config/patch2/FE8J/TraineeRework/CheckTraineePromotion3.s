@ORG 0xD14D0		@{J}
@ORG 0xCC7B4		@{U}

.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@
@�Ȃ���sp�Ƀf�[�^��ۑ����Ă���̂ŁA������g��Ȃ��悤�ɁA�������܂�
@���̃f�[�^�ֈړ����镔��

ldr  r3, Table
add  r0 ,r3, r4
ldrb r1, [r0, #0x0]
cmp  r1, r9

ldr r0, =0x080D14D8|1	@{J}
@ldr r0, =0x080CC7BC|1	@{U}
bx  r0

.align
.ltorg
Table:
@Table
