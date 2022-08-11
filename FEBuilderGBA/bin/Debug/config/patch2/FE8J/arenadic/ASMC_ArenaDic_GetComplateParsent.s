.thumb
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

@�B�����̎擾
ASMC_ArenaDic_GetComplateParsent:
push {lr, r4, r5, r6}

mov r5, #0x0	@�B���������߂�
mov r6, #0x0	@���������߂�

ldr r4, ArenaDicStruct
Loop:
ldr r0, [r4]
cmp r0, #0x0
beq Break

CheckFlag:
ldrb r0, [r4,#0x0C]	@ArenaDicStruct->ShowAlways
cmp r0, #0x0
bne IncrementAchievement

mov r0, r6
bl  CheckBit
cmp r0 ,#0x0
beq Next

IncrementAchievement:
add r5, #0x1	@�B����++

Next:
add r4, #0x14	@sizeof(ArenaDicStruct)
add r6, #0x1	@����++
b   Loop

Break:


@comp*100/all=
mov  r0, r5    @�B����
mov  r1, #100
mul  r0, r1

mov  r1, r6    @����
swi 6  @div

@�B�������������X���b�gC�Ɋi�[���ĕԂ�
ldr r3, =0x030004B0		@MemorySlot	{J}
@ldr r3, =0x030004B8		@MemorySlot	{U}
str r0, [r3, #4*0x0C]

@[G]�Ŏ���̈�ɂ�����
ldr r3, =0x03000040
str r0, [r3, #0x3c]

pop {r4,r5,r6}
pop {r1}
bx r1



CheckBit:
push {lr}
@convert to bitflag
asr r2 ,r0 ,#0x5
lsl r2 ,r2 ,#0x2

mov r1, #0x1f
and r0 ,r1

mov r1 ,#0x01
lsl r1 ,r0

ldr r3, ArenaDicConfig
ldr r3, [r3, #0x1C]	@ArenaDicConfig->RAM

ldr r0, [r3, r2]
and r0 ,r1
cmp r0, #0x0
beq CheckBit_Exit

mov r0, #0x1
CheckBit_Exit:
pop {r1}
bx r1


.ltorg
DATA:
.equ	ArenaDicStruct,	DATA+0
.equ	ArenaDicConfig,	DATA+4
