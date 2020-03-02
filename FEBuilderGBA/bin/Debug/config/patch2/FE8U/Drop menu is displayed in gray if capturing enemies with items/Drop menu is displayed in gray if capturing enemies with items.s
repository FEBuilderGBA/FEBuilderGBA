@�A�C�e���������Ă���G��capture���Ă���΁Adrop���j���[���D�F�\��
@Drop menu is displayed in Gray if capturing enemies with items


.thumb
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm

.org 0x0

PUSH {r4,r5,r6,lr}   @DropCommandPaint
MOV r6 ,r0
MOV r4 ,r1

@ldr	r1, =0x03004DF0	@����L�����̃��[�N�������ւ̃|�C���^	{J}
ldr	r1, =0x03004E50	@����L�����̃��[�N�������ւ̃|�C���^	{U}
ldr	r0, [r1, #0x0]	@UnitRAM�\����

@�G��߂炦�Ă���K�v������
ldrb r0, [r0, #0x1b]
mov r2, #0x80
and r2, r0
cmp r2, #0x0
beq Skip1

@�G��RamUnit�\���̂��~����
@blh 0x08019108   @{J}  GetUnitStruct RET=RAM Unit:@UNIT
blh 0x08019430   @{U}  GetUnitStruct RET=RAM Unit:@UNIT
cmp r0,#0x00  @���肦�Ȃ����G���[����
beq Skip1

ldrb r1, [r0,#0x1e]   @Item1
cmp r1,#0x0
beq Skip1 @���̓G�̓A�C�e���������Ă��Ȃ�

    @�G���A�C�e���������Ă���
    MOV r0 ,r4
    ADD r0, #0x34
    MOV r1, #0x1   @�D�F Gray!
@    BLH 0x08003D90 @{J} Text_SetColorId
    BLH 0x08003E60   @{U} Text_SetColorId

Skip1:
MOV r0 ,r4
ADD r0, #0x3D
LDRB r0, [r0, #0x0]
MOV r5 ,r4
ADD r5, #0x34
CMP r0, #0x2
BNE Skip2
    MOV r0 ,r5
    MOV r1, #0x1
@    BLH 0x08003D90 @{J} Text_SetColorId
    BLH 0x08003E60   @{U} Text_SetColorId

Skip2:

@LDR r0, [r4, #0x30] @{J}
@LDR r1, [r0, #0x0]  @{J}
@MOV r0 ,r5          @{J}
@BLH 0x08003F28      @{J} Text_AppendString

LDR r0, [r4, #0x30] @{U}
LDRH r0, [r0, #0x4] @{U}
BLH 0x0800A240      @{U} GetStringFromIndex
MOV r1 ,r0          @{U}
MOV r0 ,r5          @{U}
BLH 0x08004004      @{U} Text_AppendString


MOV r0 ,r6
ADD r0, #0x64
LDRB r0, [r0, #0x0]
LSL r0 ,r0 ,#0x1C
LSR r0 ,r0 ,#0x1E
@BLH 0x08001BC0     @{J} BG_GetMapBuffer
BLH 0x08001C4C     @{U} BG_GetMapBuffer
MOV r1 ,r0
MOV r2, #0x2C
LDSH r0, [r4, r2]
LSL r0 ,r0 ,#0x5
MOV r3, #0x2A
LDSH r2, [r4, r3]
ADD r0 ,r0, R2
LSL r0 ,r0 ,#0x1
ADD r1 ,r1, R0
MOV r0 ,r5
@BLH 0x08003DA0      @{J}	Text_Draw
BLH 0x08003E70      @{U} Text_Draw
POP {r4,r5,r6}
POP {r1}
BX r1
