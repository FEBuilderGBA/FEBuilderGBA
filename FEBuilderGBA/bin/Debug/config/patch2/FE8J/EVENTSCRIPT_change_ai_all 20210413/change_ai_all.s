@���̏����Ƀ}�b�`���邷�ׂĂ̓G�R��AI��SVAL1�̓��e�ɕύX
@4[0] 0D [UNIT] [CLASS] [ASM+1]
@
@���̏����Ƀ}�b�`���邷�ׂĂ̗F�R��AI��SVAL1�̓��e�ɕύX
@4[1] 0D [UNIT] [CLASS] [ASM+1]
@
@UNIT=0x00 ANY
@CLASS=0x00 ANY
@
@Author 7743
@
.align 4
.macro blh to, reg=r3
  ldr \reg, =\to
  mov lr, \reg
  .short 0xf800
.endm
@
@
.thumb
	push	{r4,r5,r6,lr}

	ldr  r4, [r0, #0x38]      @�C�x���g���߂ɃA�N�Z�X�炵�� [r0,#0x38] �ŃC�x���g���߂������Ă���A�h���X�̏ꏊ��
	ldrb r0, [r4, #0x0]       @����0 4[����0]

	mov  r1,#0xf              @check type
	and  r0,r1                
	cmp  r0,#0x00
	bne  EachAlly
	                          @EachEnemy
	ldr  r5,=0x0202CFB8       @Enemy Units
	ldr  r6,=0x0202CFB8+(0x32*0x48)       @End Address
	b Each

EachAlly:
	ldr  r5,=0x0202DDC8       @Ally Units
	ldr  r6,=0x0202DDC8+(0x14*0x48)       @End Address

Each:
	ldr  r0,[r5]              @get rom unit pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x2]       @����1 40 0D [����1] 00 [�v���O�����ꏊ XX XX XX 08]
	cmp  r2,#0x00
	beq  CheckClass
                              @CheckUnitID
	ldrb r1,[r0,#0x4]         @get unit id
	cmp  r1,r2
	bne  NextLoop

CheckClass:
	ldr  r0,[r5,#0x4]         @get rom class pointer
	cmp  r0,#0x00
	beq  NextLoop

	ldrb r2, [r4, #0x3]       @����2 40 0D 00 [����2] [�v���O�����ꏊ XX XX XX 08]
	cmp  r2,#0x00
	beq  Change
                              @CheckClassID
	ldrb r1,[r0,#0x4]         @get class id
	cmp  r1,r2
	bne  NextLoop

Change:
	ldr  r0,=0x030004B0       @�������X���b�g0�̈ʒu
	ldrb r1,[r0,#4]           @AI1�̐ݒ� [AI1][AI2]0000  (���ۂɂ̓��g���G���f�B�A��)
	ldrb r2,[r0,#5]           @AI2�̐ݒ�
	mov r0,r5                 @r0=ram unit pointer

	blh  0x08011DB0           @ChangeAI


NextLoop:
	add  r5,#0x48
	cmp  r5,r6
	blt  Each

Term:
	mov	r0, #0
	pop {pc,r6,r5,r4}
