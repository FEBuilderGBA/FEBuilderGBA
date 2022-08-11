.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@�S���N���A����
ASMC_ClearAll:
push {lr, r4, r5}

mov r5, #0x0	@���������߂�

ldr r4, ArenaDicStruct
Loop:
ldr r0, [r4]
cmp r0, #0x0
beq Break

add r4, #0x14	@sizeof(ArenaDicStruct)
add r5, #0x1	@����++
b   Loop

Break:

@��L�o�C�g�����߂�
lsl r0, r5, #0x3

mov r1, #0x7
and r1, r5
cmp r1, #0x0
beq RunMemset

add r0, #0x1

RunMemset:
mov r2, r0   @�N���A����o�C�g�̑���
mov r1, #0x0 @0�N���A�Ȃ̂�0�Œ�

ldr r0, ArenaDicConfig
ldr r0, [r0, #0x1C]	@ArenaDicConfig->RAM

blh 0x080d6968 ,r4   @memset

pop {r4, r5}
pop {r0}
bx r0


.ltorg
DATA:
.equ	ArenaDicStruct,	DATA+0
.equ	ArenaDicConfig,	DATA+4
