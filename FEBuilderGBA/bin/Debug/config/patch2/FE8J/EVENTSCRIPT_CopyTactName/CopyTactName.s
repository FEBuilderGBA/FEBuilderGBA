@�R�t�̖��O���R�s�[���Đݒ肷��
@
@40 0D [X] [Y] [TEXT]
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
	push	{lr}

	ldr  r0, [r0, #0x38]      @�C�x���g���߂ɃA�N�Z�X�炵�� [r0,#0x38] �ŃC�x���g���߂������Ă���A�h���X�̏ꏊ��
	ldrh r0, [r0, #0x2]       @����0 [TEXT]

	blh 0x08009fa8   @GetStringFromIndex	{J}
	blh 0x08031438   @SetTacticianName	{J}
Term:
	pop {pc}
